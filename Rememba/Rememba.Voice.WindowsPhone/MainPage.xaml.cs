using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Rememba.Repositories.Windows;
using Rememba.Contracts.Models;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Media;
using System.Diagnostics;
using Windows.Media.MediaProperties;
using System.ComponentModel;
using Rememba.Model;
using Microsoft.WindowsAzure.Storage.Blob;
using Windows.Storage.Streams;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Rememba.Voice.WindowsPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private MediaCapture _mediaCaptureManager;
        private StorageFile _recordStorageFile;

        private bool _recording;

        private MindMapRepository _mindMapRepository;

        public MindMapRepository MindMapRepository
        {
            get { return _mindMapRepository; }
            set { _mindMapRepository = value; }
        }

        private ObservableCollection<IMindMap> _mindMapList;

        public ObservableCollection<IMindMap> MindMapList
        {
            get { return _mindMapList; }
            set
            {
                _mindMapList = value;
                NotifyPropertyChanged("MindMapList");
            }
        }

        private ObservableCollection<string> _memoList;

        public ObservableCollection<string> MemoList
        {
            get { return _memoList; }
            set
            {
                _memoList = value;
                NotifyPropertyChanged("MemoList");
            }
        }

        private string _selectedMemo;

        public string SelectedMemo
        {
            get { return _selectedMemo; }
            set
            {
                _selectedMemo = value;
                NotifyPropertyChanged("SelectedMemo");
            }
        }

        private string _selectedMindMap;

        public string SelectedMindMap
        {
            get { return _selectedMindMap; }
            set
            {
                _selectedMindMap = value;
                NotifyPropertyChanged("SelectedMindMap");
            }
        }

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.DataContext = this;
            MemoList = new ObservableCollection<string>();
            MindMapList = new ObservableCollection<IMindMap>();
            await InitializeAudioRecording();

            await Init();

            await InitFiles();
        }

        StorageFolder memosFolder;

        private async Task InitFiles()
        {
            StorageFolder localFolder = KnownFolders.VideosLibrary;// ApplicationData.Current.LocalCacheFolder;

            if(localFolder.GetFolderAsync("Memos").Status == AsyncStatus.Error)
            {
               await localFolder.CreateFolderAsync("Memos");
            }


            memosFolder = await localFolder.GetFolderAsync("Memos");
            var fileList = await memosFolder.GetFilesAsync();

            foreach (var file in fileList)
            {
                MemoList.Add(file.Name);
            }
        }

        private async Task Init()
        {
            MindMapRepository = new MindMapRepository();

            var mindMapList = await MindMapRepository.ListMindMaps();

            foreach (MindMap map in mindMapList)
            {
                MindMapList.Add(map);
            }
        }

        private async void record_Click(object sender, RoutedEventArgs e)
        {
            await RecordAudio();

            record.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            stop.Visibility = Windows.UI.Xaml.Visibility.Visible;
            play.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            stopPlay.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private async void play_Click(object sender, RoutedEventArgs e)
        {
            await PlayRecorded();
            record.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            stop.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            play.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            stopPlay.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private async void stop_Click(object sender, RoutedEventArgs e)
        {
            await StopRecording();

            record.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            stop.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            play.Visibility = Windows.UI.Xaml.Visibility.Visible;
            stopPlay.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private async Task<bool> PersistRecording(string memoName)
        {
            string blobName = memoName + ".m4a";
           

            if (await StorageHelper.DoesFileExistAsync
                (blobName, memosFolder))
            {

                StorageFile file = await memosFolder.GetFileAsync(blobName);

                var fileStream = await file.OpenSequentialReadAsync();
                await UploadBinaryContent(memoName, (FileInputStream)fileStream);

                return true;
            }
            //ContentRepository repo = new ContentRepository();
            //var content = new Content()
            //{
            //    Id = recordingName + Guid.NewGuid().ToString(),

            //    Data = ""
            //};

            //repo.AddContent(content);


            return false;
        }

        public async Task UploadBinaryContent(string name, Windows.Storage.Streams.FileInputStream stream)
        {

            CloudBlobContainer container = new CloudBlobContainer(new Uri(Settings.TenantBinaryContentContainerSaS));
            CloudBlockBlob blob = container.GetBlockBlobReference(name);
            await blob.DeleteIfExistsAsync();

            blob.Properties.ContentType = "audio/mp4";
            await blob.UploadFromStreamAsync(stream);
        }

        private void stopPlay_Click(object sender, RoutedEventArgs e)
        {
            record.Visibility = Windows.UI.Xaml.Visibility.Visible;
            stop.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            play.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            stopPlay.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            StopPlayback();

        }

        private async Task InitializeAudioRecording()
        {
            record.Visibility = Windows.UI.Xaml.Visibility.Visible;
            stop.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            play.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            stopPlay.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            _mediaCaptureManager = new MediaCapture();
            var settings = new MediaCaptureInitializationSettings();
            settings.StreamingCaptureMode = StreamingCaptureMode.Audio;
            settings.MediaCategory = MediaCategory.Other;
            settings.AudioProcessing = AudioProcessing.Default;

            await _mediaCaptureManager.InitializeAsync(settings);

            _mediaCaptureManager.RecordLimitationExceeded += _mediaCaptureManager_RecordLimitationExceeded;
            _mediaCaptureManager.Failed += _mediaCaptureManager_Failed;
        }

        void _mediaCaptureManager_Failed(MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs)
        {
            throw new NotImplementedException();
        }

        void _mediaCaptureManager_RecordLimitationExceeded(MediaCapture sender)
        {
            throw new NotImplementedException();
        }

        private async Task RecordAudio()
        {
            try
            {
                Debug.WriteLine("Starting record");
                String fileName = DateTime.Now.ToString("yyyyMMddHHmmss.raw");

                MemoList.Add(fileName);

                _recordStorageFile = await memosFolder.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);

                Debug.WriteLine("Create record file successfully");

                MediaEncodingProfile recordProfile = MediaEncodingProfile.CreateM4a(AudioEncodingQuality.Low);
                await _mediaCaptureManager.StartRecordToStorageFileAsync(recordProfile, this._recordStorageFile);

                Debug.WriteLine("Start Record successful");

                _recording = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to capture audio");
            }
        }

        private async Task StopRecording()
        {

            if (_recording)
            {
                Debug.WriteLine("Stopping recording");
                await _mediaCaptureManager.StopRecordAsync();
                Debug.WriteLine("Stop recording successful");
                _recording = false;
            }

        }

        private async Task PlayRecorded()
        {
            if (!_recording)
            {
                var stream = await _recordStorageFile.OpenAsync(FileAccessMode.Read);
                Debug.WriteLine("Recording file opened");
                playbackElement1.AutoPlay = true;
                playbackElement1.SetSource(stream, _recordStorageFile.FileType);
                playbackElement1.Play();
            }
        }

        private void StopPlayback()
        {
            playbackElement1.Play();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach(string memo in MemoList)
            {                 var stream = await _recordStorageFile.OpenAsync(FileAccessMode.Read);
                UploadBinaryContent(memo,)
            }
        }


    }
}

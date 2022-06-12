using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Daw.Player
{
    public class WasapiPlayer
    {
        AudioFileReader _fileReader;
        SampleChannel _sampleChannel;
        Dictionary<string, MMDevice> _devices = new();
        string _deviceName;
        MMDevice _mmDevice;
        IWavePlayer _waveOut;
        string _fileName;

        public WasapiPlayer()
        {
            var enumerator = new MMDeviceEnumerator();
            var endPoints = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            foreach (var endPoint in endPoints) _devices.Add($"{endPoint.FriendlyName} ({endPoint.DeviceFriendlyName})", endPoint);
            //IsReady = false;
        }
        ~WasapiPlayer() => Dispose();

        public event EventHandler<StoppedEventArgs> PlaybackStopped;

        public bool IsReady { get; private set; }

        public TimeSpan TotalTime => _fileReader.TotalTime;

        public event EventHandler FileNameUpdated;

        public string FileName
        {
            get => _fileName ?? string.Empty;
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    IsReady = false;
                    if (!string.IsNullOrEmpty(_fileName) && File.Exists(_fileName)) IsReady = Create();
                    FileNameUpdated?.Invoke(this, null);
                }
            }
        }

        void Dispose()
        {
            if (_waveOut != null && (_waveOut.PlaybackState == PlaybackState.Playing || _waveOut.PlaybackState == PlaybackState.Paused)) _waveOut.Stop();
            if (_fileReader != null) { _fileReader.Dispose(); _fileReader = null; } // this one really closes the file and ACM conversion
            if (_waveOut != null) { _waveOut.Dispose(); _waveOut = null; }
        }

        bool Create()
        {
            Dispose();

            ISampleProvider sampleProvider = null;
            try
            {
                sampleProvider = CreateInputStream(_fileName);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error Loading File"); return false; }

            try
            {
                const int LATENCY_MS = 25;
                _waveOut = new WasapiOut(_mmDevice, AudioClientShareMode.Shared, true, LATENCY_MS);
                _waveOut.PlaybackStopped += OnPlaybackStopped;
                _waveOut.Init(sampleProvider);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error Initializing Output"); return false; }
            return true;
        }

        void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (_fileReader != null) _fileReader.Position = 0;
            PlaybackStopped?.Invoke(this, e);
        }

        public PlaybackState PlayBackState => _waveOut == null ? PlaybackState.Stopped : _waveOut.PlaybackState;

        public void Play() { if (_waveOut != null && _waveOut.PlaybackState != PlaybackState.Playing) _waveOut.Play(); }
        public void Stop() => _waveOut?.Stop();
        public void Pause() { if (_waveOut != null && _waveOut.PlaybackState != PlaybackState.Playing) _waveOut.Pause(); }

        public TimeSpan CurrentTime
        {
            get => _waveOut != null && _fileReader != null && _waveOut.PlaybackState != PlaybackState.Stopped ? _fileReader.CurrentTime : TimeSpan.Zero;
            set { if (_waveOut != null) { _fileReader.CurrentTime = value; } }
        }

        float _volume;
        public float Volume
        {
            get => _volume;
            set { _volume = value; if (_sampleChannel != null) _sampleChannel.Volume = value; }
        }

        ISampleProvider CreateInputStream(string fileName)
        {
            _fileReader = new AudioFileReader(fileName);
            _sampleChannel = new SampleChannel(_fileReader, true);
            _sampleChannel.PreVolumeMeter += OnPreVolumeMeter;
            _sampleChannel.Volume = Volume;
            var postVolumeMeter = new MeteringSampleProvider(_sampleChannel);
            postVolumeMeter.StreamVolume += OnPostVolumeMeter;
            return postVolumeMeter;
        }

        public event EventHandler<StreamVolumeEventArgs> PostVolumeMeter;
        void OnPostVolumeMeter(object sender, StreamVolumeEventArgs e) => PostVolumeMeter?.Invoke(this, e);

        public event EventHandler<StreamVolumeEventArgs> PreVolumeMeter;
        void OnPreVolumeMeter(object sender, StreamVolumeEventArgs e) => PreVolumeMeter?.Invoke(this, e);

        public string[] AvailableDeviceNames => _devices.Keys.ToArray();

        public string DeviceName
        {
            get => _deviceName;
            set
            {
                if (!_devices.ContainsKey(value)) return;
                Dispose(); // Stop & cleanup in case it was running
                OnPlaybackStopped(this, new StoppedEventArgs());
                _deviceName = value;
                _mmDevice = _devices[value];
            }
        }
    }
}
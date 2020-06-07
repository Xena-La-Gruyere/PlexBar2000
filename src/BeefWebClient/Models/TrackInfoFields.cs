using System.ComponentModel;

namespace BeefWebClient.Models
{
    public enum TrackInfoFields
    {
        [Description("%album artist%")]
        AlbumArtist,
        [Description("%album%")]
        Album,
        [Description("%artist%")]
        Artist,
        [Description("%discnumber%")]
        Discnumber,
        [Description("%totaldiscs%")]
        Totaldiscs,
        [Description("%track artist%")]
        TrackArtist,
        [Description("%title%")]
        Title,
        [Description("%tracknumber%")]
        TracknumberTwoDigit,
        [Description("%track number%")]
        Tracknumber,
        [Description("%date%")]
        Date,

        
        [Description("%bitrate%")]
        Bitrate,
        [Description("%channels%")]
        Channels,
        [Description("%codec%")]
        Codec,
        [Description("%filesize%")]
        Filesize,
        [Description("%filesize_natural%")]
        FilesizeNatural,
        [Description("%length%")]
        Length,
        [Description("%length_ex%")]
        LengthEx,
        [Description("%length_seconds%")]
        LenghtSeconds,
        [Description("%length_seconds_fp%")]
        LenghtSecondsFloatingPoint,
        [Description("%length_samples%")]
        LenghtSamples,
        [Description("%samplerate%")]
        SampleRate,


        [Description("%filename%")]
        FileNameWithoutExtension,
        [Description("%filename_ext%")]
        FileName,
        [Description("%directoryname%")]
        ParentDirectoryName,
        [Description("%last_modified%")]
        LastModified,
        [Description("%path%")]
        Path,
        [Description("%_path_raw%")]
        PathUrl,
        [Description("%subsong%")]
        SubsongIndex,
        [Description("%_foobar2000_version%")]
        Foobar2000Version,


        
        [Description("%isplaying%")]
        IsPlaying,
        [Description("%ispaused%")]
        IsPaused,
    }
}

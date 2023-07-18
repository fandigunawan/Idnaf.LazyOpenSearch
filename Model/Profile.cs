using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Idnaf.LazyOpenSearch.Model
{
    /// <summary>
    /// Authentication mode
    /// </summary>
    public enum AuthMode
    {
        [EnumMember(Value="UsernamePassword")]
        UsernamePassword,
        [EnumMember(Value = "MutualAuth")]
        MutualAuth
    }

    public enum DebugMode
    {
        [EnumMember(Value = "None")]
        None,
        [EnumMember(Value = "Console")]
        Console,
        [EnumMember(Value = "File")]
        File
    }
    public class Auth
    {
        public AuthMode Mode { get; set; } = AuthMode.UsernamePassword;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string AuthFile { get; set; } = string.Empty;
        public string AuthFilePassword { get; set; } = string.Empty;
    }
    public class Profile
    {
        public string Name { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public Auth Authentication { get; set; } = new Auth();
        public bool IgnoreTLSCertValidation { get; set; } = true;
        public DebugMode DebugMode { get; set; } = DebugMode.None;
        public string DebugLogFile { get; set; } = string.Empty;

    }

    public class Profiles
    {
        public Profile[] profiles = Array.Empty<Profile>();
    }
}

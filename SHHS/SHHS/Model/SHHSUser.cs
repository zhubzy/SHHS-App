namespace SHHS.Model
{
    public class SHHSLogin
    {
 
            public string Username { get; set; }
            public string Password { get; set; }


    }

    public class SHHSUser {


        public string UID { get; set; }
        public string DeviceName { get; set; }
        public string Model { get; set; }
        public string Manufacture { get; set; }
        public string Version { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }



    }



    public class SHHSAppInfo { 
            

        public string iOSBuild { get; set; }
        public string iOSVersion { get; set; }
        public string andriodBuild { get; set; }
        public string andriodVersion { get; set; }


    }
}

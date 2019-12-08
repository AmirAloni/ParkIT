using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;


namespace ParkIT.Services.DB_Service
{
    public class DataBaseService
    {
    
        public byte[] DownloadImage(string i_SignString)
        {
            SqlConnection sqlConnection = new SqlConnection("Data Source = parkitsqlserver.database.windows.net; Initial Catalog = ParkITDB; Persist Security Info = True; User ID = amiral; Password = Aa130492");
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("SELECT SignImage FROM TrafficSigns WHERE SignString = @param", sqlConnection);
            SqlParameter myparam = command.Parameters.Add("@param", SqlDbType.NVarChar);
            myparam.Value = i_SignString;
            byte[] byteImage = (byte[])command.ExecuteScalar();
            sqlConnection.Close();
            return byteImage;
        }

        public bool IsSignExists(string i_SignString)
        {
            SqlConnection sqlConnection = new SqlConnection("Data Source = parkitsqlserver.database.windows.net; Initial Catalog = ParkITDB; Persist Security Info = True; User ID = amiral; Password = Aa130492");
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("SELECT SignString FROM TrafficSigns WHERE SignString = @param", sqlConnection);
            SqlParameter myparam = command.Parameters.Add("@param", SqlDbType.NVarChar);
            myparam.Value = i_SignString;
            var reader = command.ExecuteReader();

            if (reader.HasRows)
                return true;

            reader.Close();
            sqlConnection.Close();

            return false;
        }

        public async void PickImageuploadSign(string i_SignString)
        {
            byte[] imageBytes = null;

            var imageFile = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });

            FileStream fileStream = new FileStream(imageFile.Path, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            imageBytes = binaryReader.ReadBytes((int)fileStream.Length);

            UploadSign(imageBytes, i_SignString);
        }

        public void UploadSign(byte[] i_ImageBytes, string SignString)
        {
            SqlConnection sqlConnection = new SqlConnection("Data Source = parkitsqlserver.database.windows.net; Initial Catalog = ParkITDB; Persist Security Info = True; User ID = amiral; Password = Aa130492");
            SqlCommand command = new SqlCommand(string.Format("INSERT INTO TrafficSigns (SignString, SignImage) VALUES ('{0}',@imageBytes)", SignString), sqlConnection);
            command.Parameters.Add(new SqlParameter("@imageBytes", i_ImageBytes));
            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }

    }
}

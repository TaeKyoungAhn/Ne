using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif
using UnityEngine.UI;


public class CaptureScreen : MonoBehaviour
{
    public Button screenShotButton;          // ��ü ȭ�� ĸ��

    private Texture2D _screenShotTexture;

    public ScreenShotFlash flash;

    public string folderName = "ScreenShots";
    public string fileName = "MyScreenShot";
    public string extName = "png";

    private string RootPath
    {
        get
        {
#if UNITY_ANDROID
            return $"/storage/emulated/0/DCIM/{Application.productName}/";
            //return Application.persistentDataPath;
#elif   UNITY_EDITOR || UNITY_STANDALONE
            return Application.dataPath;
#endif
        }
    }
    private string FolderPath => $"{RootPath}/{folderName}";
    private string TotalPath => $"{FolderPath}/{fileName}_{DateTime.Now.ToString("MMdd_HHmmss")}.{extName}";

    private string lastSavedPath;


    private void OnEnable()
    {
        screenShotButton.onClick.AddListener(TakeScreenShotFull);
    }


    private void TakeScreenShotFull()
    {
#if UNITY_ANDROID
        CheckAndroidPermissionAndDo(Permission.ExternalStorageWrite, () => StartCoroutine(TakeScreenShotRoutine()));
#else
        StartCoroutine(TakeScreenShotRoutine());
#endif
    }


    private IEnumerator TakeScreenShotRoutine()
    {
        yield return new WaitForEndOfFrame();
        CaptureScreenAndSave();
    }

#if UNITY_ANDROID
    /// <summary> �ȵ���̵� - ���� Ȯ���ϰ�, ���ν� ���� �����ϱ� </summary>
    private void CheckAndroidPermissionAndDo(string permission, Action actionIfPermissionGranted)
    {
        // �ȵ���̵� : ����� ���� Ȯ���ϰ� ��û�ϱ�
        if (Permission.HasUserAuthorizedPermission(permission) == false)
        {
            PermissionCallbacks pCallbacks = new PermissionCallbacks();
            pCallbacks.PermissionGranted += str => Debug.Log($"{str} ����");
            
            pCallbacks.PermissionGranted += _ => actionIfPermissionGranted(); // ���� �� ��� ����

            pCallbacks.PermissionDenied += str => Debug.Log($"{str} ����");
           
            pCallbacks.PermissionDeniedAndDontAskAgain += str => Debug.Log($"{str} ���� �� �ٽô� ���� ����");
           

            Permission.RequestUserPermission(permission, pCallbacks);
        }
        else
        {
            actionIfPermissionGranted(); // �ٷ� ��� ����
        }
    }
#endif


    private void CaptureScreenAndSave()
    {
        string totalPath = TotalPath; // ������Ƽ ���� �� �ð��� ���� �̸��� �����ǹǷ� ĳ��

        Texture2D screenTex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        Rect area = new Rect(0f, 0f, Screen.width, Screen.height);

        // ���� ��ũ�����κ��� ���� ������ �ȼ����� �ؽ��Ŀ� ����
        screenTex.ReadPixels(area, 0, 0);

        bool succeeded = true;
        try
        {
            // ������ �������� ������ ���� ����
            if (Directory.Exists(FolderPath) == false)
            {
                Directory.CreateDirectory(FolderPath);
            }

            // ��ũ���� ����
            File.WriteAllBytes(totalPath, screenTex.EncodeToPNG());
        }
        catch (Exception e)
        {
            succeeded = false;
            Debug.LogWarning($"Screen Shot Save Failed : {totalPath}");
            Debug.LogWarning(e);
        }

        // ������ �۾�
        Destroy(screenTex);

        if (succeeded)
        {
            Debug.Log($"Screen Shot Saved : {totalPath}");
            flash.Show(); // ȭ�� ��½
            lastSavedPath = totalPath; // �ֱ� ��ο� ����
        }

        // ������ ����
        RefreshAndroidGallery(totalPath);
    }

    [System.Diagnostics.Conditional("UNITY_ANDROID")]
    private void RefreshAndroidGallery(string imageFilePath)
    {
#if !UNITY_EDITOR
        AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass classUri = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject objIntent = new AndroidJavaObject("android.content.Intent", new object[2]
        { "android.intent.action.MEDIA_SCANNER_SCAN_FILE", classUri.CallStatic<AndroidJavaObject>("parse", "file://" + imageFilePath) });
        objActivity.Call("sendBroadcast", objIntent);
#endif
    }
}

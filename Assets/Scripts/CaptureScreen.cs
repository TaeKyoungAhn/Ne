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
    public Button screenShotButton;          // 전체 화면 캡쳐

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
    /// <summary> 안드로이드 - 권한 확인하고, 승인시 동작 수행하기 </summary>
    private void CheckAndroidPermissionAndDo(string permission, Action actionIfPermissionGranted)
    {
        // 안드로이드 : 저장소 권한 확인하고 요청하기
        if (Permission.HasUserAuthorizedPermission(permission) == false)
        {
            PermissionCallbacks pCallbacks = new PermissionCallbacks();
            pCallbacks.PermissionGranted += str => Debug.Log($"{str} 승인");
            
            pCallbacks.PermissionGranted += _ => actionIfPermissionGranted(); // 승인 시 기능 실행

            pCallbacks.PermissionDenied += str => Debug.Log($"{str} 거절");
           
            pCallbacks.PermissionDeniedAndDontAskAgain += str => Debug.Log($"{str} 거절 및 다시는 보기 싫음");
           

            Permission.RequestUserPermission(permission, pCallbacks);
        }
        else
        {
            actionIfPermissionGranted(); // 바로 기능 실행
        }
    }
#endif


    private void CaptureScreenAndSave()
    {
        string totalPath = TotalPath; // 프로퍼티 참조 시 시간에 따라 이름이 결정되므로 캐싱

        Texture2D screenTex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        Rect area = new Rect(0f, 0f, Screen.width, Screen.height);

        // 현재 스크린으로부터 지정 영역의 픽셀들을 텍스쳐에 저장
        screenTex.ReadPixels(area, 0, 0);

        bool succeeded = true;
        try
        {
            // 폴더가 존재하지 않으면 새로 생성
            if (Directory.Exists(FolderPath) == false)
            {
                Directory.CreateDirectory(FolderPath);
            }

            // 스크린샷 저장
            File.WriteAllBytes(totalPath, screenTex.EncodeToPNG());
        }
        catch (Exception e)
        {
            succeeded = false;
            Debug.LogWarning($"Screen Shot Save Failed : {totalPath}");
            Debug.LogWarning(e);
        }

        // 마무리 작업
        Destroy(screenTex);

        if (succeeded)
        {
            Debug.Log($"Screen Shot Saved : {totalPath}");
            flash.Show(); // 화면 번쩍
            lastSavedPath = totalPath; // 최근 경로에 저장
        }

        // 갤러리 갱신
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

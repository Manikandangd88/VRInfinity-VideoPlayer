using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VideoPlayerManager : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    
    private enum playerState
    {
        empty = 0,
        play, pause, stop
    };


    #region Private Fields

    [SerializeField] private Button playBtn;
    [SerializeField] private Button stopBtn;

    [SerializeField] private Slider timeSlider;

    [SerializeField] private bool isDragging = false;

    private VideoPlayer videoPlayer;

    [SerializeField] private Image progressBar;

    #endregion

    #region Listeners

    #endregion

    //https://unity3dcollege.blob.core.windows.net/site/YTDownloads/test.mp4

    private playerState videoPlayerState = playerState.empty;

    private void InitFields()
    {
        playBtn.onClick.AddListener(() => StartVideoPlay(playerState.play));
        stopBtn.onClick.AddListener(() => StartVideoPlay(playerState.stop));

        //timeSlider.onValueChanged.AddListener(HandleTimeSliderValueChanged);

        videoPlayer = GetComponent<VideoPlayer>();
    }

    private void HandleTimeSliderValueChanged(float value)
    {

    }

    //public void BeginDrag()
    //{
    //    isDragging = true;
    //}

    //public void DragEnd()
    //{
    //    isDragging = false;
    //}

    private void StartVideoPlay(playerState videostate)
    {
        switch (videostate)
        {
            case playerState.empty:
                break;
            case playerState.play:
                videoPlayer.Play();
                break;
            case playerState.pause:
                videoPlayer.Pause();
                break;
            case playerState.stop:
                videoPlayer.Stop();
                break;
            default:
                Debug.Log("inside default case");
                break;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitFields();
    }

    // Update is called once per frame
    void Update()
    {
        if (videoPlayer.frameCount > 0)
            progressBar.fillAmount = (float)videoPlayer.frame / (float)videoPlayer.frameCount;
    }

    public void OnDrag(PointerEventData eventData)
    {
        TrySkip(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TrySkip(eventData);
    }

    private void TrySkip(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            progressBar.rectTransform, eventData.position, null, out localPoint))
        {
            float pct = Mathf.InverseLerp(progressBar.rectTransform.rect.xMin, progressBar.rectTransform.rect.xMax, localPoint.x);
            SkipToPercent(pct);
        }
    }

    private void SkipToPercent(float pct)
    {
        var frame = videoPlayer.frameCount * pct;
        videoPlayer.frame = (long)frame;
    }
}

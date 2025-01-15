using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;

namespace WonderBingo
{
    public class GridButton : MonoBehaviour
    {
        #region Private Variables
        [Header("IsAnswered")]
        [SerializeField] private bool isAnswered;
        [SerializeField] private string answer;

        [Header("Locked Panel")]
        [SerializeField] private GameObject lockedPanel;

        [SerializeField] private Sprite _tickImage;
        [SerializeField] private Sprite _wrongImage;

        [Header("OneLiner")]
        [SerializeField] private Image _oneLinerDescriptionBox;

        private Tween currentTween; // Store the current tween to control it

        public bool IsAnswered { get { return isAnswered; }  set { isAnswered = value; } }

        public string Answer { get {  return answer; } set { answer = value; } }

        public string descriptionText;

        #endregion

        public void ResetGridAnswerStatus()
        {
            isAnswered = false;
        }

        public void ResetGridAnswer()
        {
            answer = string.Empty;
            descriptionText = string.Empty;
        }

        public void EnableLockedPanel(bool _correct)
        {
            lockedPanel.SetActive(true);
            
            lockedPanel.GetComponent<Image>().sprite = _correct ? _tickImage : _wrongImage;

            lockedPanel.GetComponent<Transform>().DOScale(Vector3.one, 1f).SetEase(Ease.InOutQuad);

            if(!_correct)
            {
                StartCoroutine(DeactiveTheLockedPanel());

                IEnumerator DeactiveTheLockedPanel()
                {
                    yield return new WaitForSeconds(2f);
                    DeactivatedLockedPanel();
                }
            }

        }

        public void DeactivatedLockedPanel()
        {
            lockedPanel.SetActive(false);
            lockedPanel.transform.localScale = Vector3.zero;
        }


        // Open the popup with an animation
        public void OpenOneLiner()
        {
            _oneLinerDescriptionBox.gameObject.SetActive(true);
            _oneLinerDescriptionBox.transform.GetComponentInChildren<Text>().text = descriptionText;
            // Kill any existing tween before starting a new one
            if (currentTween != null && currentTween.IsActive())
            {
                currentTween.Kill(); // Kill any previous tween
            }
            currentTween = _oneLinerDescriptionBox.GetComponent<RectTransform>()
                .DOScale(1f, 1f)
                .SetEase(Ease.Linear);
        }

        public void CloseOneLiner()
        {
            // Kill any existing tween before starting a new one
            if (currentTween != null && currentTween.IsActive())
            {
                currentTween.Kill(); // Kill any previous tween
            }

            currentTween = _oneLinerDescriptionBox.GetComponent<RectTransform>()
                .DOScale(0f, 1f)
                .SetEase(Ease.Linear)
                .OnComplete(HideObject);  // Call HideObject once the animation is complete
        }

        // Hide the popup after tween completes
        private void HideObject()
        {
            _oneLinerDescriptionBox.gameObject.SetActive(false);
        }

        // Optional: Handle mouse out event to close the popup
        private void OnMouseExit()
        {
            // If the mouse exits while the animation is in progress, close the popup
            CloseOneLiner();
        }

    }
}

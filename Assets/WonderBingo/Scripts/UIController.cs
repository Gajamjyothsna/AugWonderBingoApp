using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;


namespace WonderBingo
{
    public class UIController : MonoBehaviour
    {
        #region Private Variables
        [Header("MainMenu Panel")]
        [SerializeField] private GameObject _mainMenuPanel;
        [Header("ContientPanel")]
        [SerializeField] private GameObject _continentPanel;
        [Header("InstructionsPanel")]
        [SerializeField] private GameObject _instructionsPanel;
        [Header("ChoronsTerraPanel")]
        [SerializeField] private GameObject _chronosTerraPanel;
        [Header("GridPanel")]
        [SerializeField] private GameObject _gridPanel;
        [Header("Bingo Panel")]
        [SerializeField] private GameObject _bingoPanel;
        [Header("MemorbiliaPanel")]
        [SerializeField] private GameObject _memorbiliaPanel;
        [Header("Memorbilia Reward Panel")]
        [SerializeField] private GameObject _memorbiliaRewardPanel;


        [Header("Grid UI Sprites")]
        [SerializeField] private List<DataModel.ContinentGridImageData> continentGridImageDataList;

        [Header("Grid UI References")]
        [SerializeField] private List<Image> gridUIImages;

        [Header("Grid Panel Dyanamic Question")]
        [SerializeField] private Text _gridPanelQuestionTMP;

        [Header("Grid Panel Buttons")]
        [SerializeField] private List<GridButton> _gridButtons;

        [Header("Data Loader")]
        [SerializeField] private DataLoader _dataLoader;

        [Header("Memorabilia Model")]
        [SerializeField] private List<DataModel.MemorabiliaModel> memorabiliaModelList;

        [Header("Memorabilia Reward Image")]
        [SerializeField] private Image _memorbiliaRewardIM;
        [SerializeField] private Text _memorbiliaRewardKeychainTMP;

        [Header("Bingo Line Images")]
        [SerializeField] private GameObject firstColumn;
        [SerializeField] private GameObject secondColumn;
        [SerializeField] private GameObject thirdColumn;
        [SerializeField] private GameObject firstRow;
        [SerializeField] private GameObject secondRow;
        [SerializeField] private GameObject thirdRow;
        [SerializeField] private GameObject diagonalLeft;
        [SerializeField] private GameObject diagonalRight;

        [Header("Grid BG")]
        [SerializeField] private Image gridBG;

        [Header("InGameAudioSource")]
        [SerializeField] private AudioSource _inGameAudioSource;
        [SerializeField] private AudioClip _rightAnswerClip;
        [SerializeField] private AudioClip _wrongAnswerClip;
        [SerializeField] private AudioClip _congratsClip;


        private DataModel.SubType _subContinentType;
        private int _subContinentIndex;
        private int _continentIndex;
        private DataModel.Continent _continent;
        #endregion

        #region Button Actions
        public void PlayButtonClick()
        {
            _mainMenuPanel.SetActive(false);
            _continentPanel.SetActive(true);
        }

        public void ContinentButtonClick(int _index)
        {
            DataModel.Continent continent = (DataModel.Continent)_index;

            _continent = continent;

            Debug.Log("Continent" + _continent);

            _continentPanel.SetActive(false);
            _instructionsPanel.SetActive(true);
        }

        public void ChronosButtonClick()
        {
            _subContinentType = DataModel.SubType.Chronos;

            _chronosTerraPanel.SetActive(false);

            EnableGridPanel();
        }

        private int _gridIndex;
        private int _randomQuestionIndex;
        public void TerraButtonClick()
        {
            _subContinentType = DataModel.SubType.Terra;
            _chronosTerraPanel.SetActive(false);

            EnableGridPanel();
        }

        public void GridButtonClick(int index)
        {
            //Validatation of the Question need to check
            Debug.Log("_randomQuestionIndex in GridButtonClick" + _randomQuestionIndex);
            if (_gridButtons[index].Answer == _dataLoader.gridData.grids[_gridIndex].questions[_randomQuestionIndex].answer)
            {
                Debug.LogError("If cASE");

                if (!_gridButtons[index].IsAnswered)
                {
                    _gridButtons[index].IsAnswered = true;

                    _gridButtons[index].EnableLockedPanel(true);

                    _inGameAudioSource.clip = _rightAnswerClip;
                    _inGameAudioSource.Play();

                    // Check for Bingo
                    int verticalRow = CheckVerticalRow();
                    int horizontalRow = CheckHorizontalRow(); // You can implement this similarly to CheckVerticalRow
                    int diagonal = CheckDigonals(); // Also modified to return an identifier if needed

                    if (verticalRow > 0 || horizontalRow > 0 || diagonal > 0)
                    {
                        //Enable the Bingo Indicator
                        Debug.LogError("Enable The Bingo Indicator");

                        if (verticalRow > 0)
                        {
                            Debug.LogError("Vertical Row" + verticalRow);
                            DeactiveBingoLineImages();
                            switch (verticalRow)
                            {
                                case 1:
                                    firstColumn.SetActive(true);
                                    firstColumn.transform.DOScale(Vector3.one, 1f);
                                    break;
                                case 2:
                                    secondColumn.SetActive(true);
                                    secondColumn.transform.DOScale(Vector3.one, 1f);
                                    break;
                                case 3:
                                    thirdColumn.SetActive(true);
                                    thirdColumn.transform.DOScale(Vector3.one, 1f);
                                    break;
                            }

                            StartCoroutine(DelayToEnableBingoPanel());
                        }
                        else if (horizontalRow > 0)
                        {
                            Debug.LogError("horizontalRow" + horizontalRow);
                            DeactiveBingoLineImages();
                            switch (horizontalRow)
                            {
                                case 1:
                                    firstRow.SetActive(true);
                                    firstRow.transform.DOScale(Vector3.one, 1f);
                                    break;
                                case 2:
                                    secondRow.SetActive(true);
                                    secondRow.transform.DOScale(Vector3.one, 1f);
                                    break;
                                case 3:
                                    thirdRow.SetActive(true);
                                    thirdRow.transform.DOScale(Vector3.one, 1f);
                                    break;
                            }
                            StartCoroutine(DelayToEnableBingoPanel());
                        }
                        else if (diagonal > 0)
                        {
                            Debug.LogError("diagonal" + diagonal);
                            DeactiveBingoLineImages();
                            switch (diagonal)
                            {
                                case 1:
                                    diagonalLeft.SetActive(true);
                                    diagonalLeft.transform.DOScale(Vector3.one, 1f);
                                    break;
                                case 2:
                                    diagonalRight.SetActive(true);
                                    diagonalRight.transform.DOScale(Vector3.one, 1f);
                                    break;
                            }
                            StartCoroutine(DelayToEnableBingoPanel());
                        }
                    }
                    else
                    {
                        //Display the next Question
                        DisplayNextRandomQuestion();
                    }
                }

                //else if (_gridButtons[index].IsAnswered)
                //{
                //    _gridButtons[index].EnableLockedPanel();
                //}
            }
            else
            {

                //if (_gridButtons[index].IsAnswered)
                //{
                //    _gridButtons[index].EnableLockedPanel();
                //}

                //Display the next Question
                _gridButtons[index].EnableLockedPanel(false);

                _inGameAudioSource.clip = _wrongAnswerClip; _inGameAudioSource.Play();

            }
            
        }

        private IEnumerator DelayToEnableBingoPanel()
        {
            yield return new WaitForSeconds(2f);
            EnableBingoIndicator();
        }

        #region Bingo Sequence

        //Bingo Sequence
        // 0    1    2
        // 3    4    5
        // 6    7    8

        private int CheckHorizontalRow() 
        {
            if (_gridButtons[0].IsAnswered && _gridButtons[1].IsAnswered && _gridButtons[2].IsAnswered)
                return 1;
            else if (_gridButtons[3].IsAnswered && _gridButtons[4].IsAnswered && _gridButtons[5].IsAnswered)
                return 2;
            else if (_gridButtons[6].IsAnswered && _gridButtons[7].IsAnswered && _gridButtons[8].IsAnswered)
                return 3;
            else
                return 0;
        }

        private int CheckVerticalRow() 
        {
            if (_gridButtons[0].IsAnswered && _gridButtons[3].IsAnswered && _gridButtons[6].IsAnswered)
                return 1;
            else if (_gridButtons[1].IsAnswered && _gridButtons[4].IsAnswered && _gridButtons[7].IsAnswered)
                return 2;
            else if (_gridButtons[2].IsAnswered && _gridButtons[5].IsAnswered && _gridButtons[8].IsAnswered)
                return 3;
            else
                return 0;
        }

        private int CheckDigonals()
        {
            if (_gridButtons[0].IsAnswered && _gridButtons[4].IsAnswered && _gridButtons[8].IsAnswered)
                return 1;
            else if (_gridButtons[2].IsAnswered && _gridButtons[4].IsAnswered && _gridButtons[6].IsAnswered)
                return 2;
            else
                return 0;
        }
        #endregion

        public void DisplayNextRandomQuestion()
        {
            // Ensure there are questions left to display
            if (_dataLoader.gridData.grids[_gridIndex].questions.All(q => q.questionalreadyShown))
            {
                _gridPanelQuestionTMP.text = "All questions have been displayed.";
                _gridPanel.SetActive(false);
                _continentPanel.SetActive(true);
                return;
            }

            int randomIndex;
            do
            {
                // Pick a random index from the entire list
                randomIndex = GenerateNextRandomIndex();
                Debug.LogError("RandomIndex in DisplayNextRandomQuestion: " + randomIndex);

            } while (_dataLoader.gridData.grids[_gridIndex].questions[randomIndex].questionalreadyShown);

            // Once we have an unshown question, display it
            var selectedQuestion = _dataLoader.gridData.grids[_gridIndex].questions[randomIndex];
            _gridPanelQuestionTMP.text = selectedQuestion.question.ToUpper();

            // Mark it as shown
            selectedQuestion.questionalreadyShown = true;

            // Log the selected index for debugging
            _randomQuestionIndex = randomIndex;
            Debug.LogError("_randomQuestionIndex in DisplayNextRandomQuestion: " + _randomQuestionIndex);
        }

        private int GenerateNextRandomIndex()
        {
            return Random.Range(0, _dataLoader.gridData.grids[_gridIndex].questions.Length);
        }

        private void EnableBingoIndicator()
        {
            _bingoPanel.SetActive(true);

            StartCoroutine(DelayToDeactiveBingoPanel());
        }

        IEnumerator DelayToDeactiveBingoPanel()
        {
            yield return new WaitForSeconds(1f);
            _bingoPanel.SetActive(false);
            EnableMemorbiliaRewardPanel();
        }

        private void DisplayMemorbiliaCollection()
        {
            for(int i=0;i<memorabiliaModelList.Count;i++)
            {
                memorabiliaModelList[i]._memorabiliaTMP.text = memorabiliaModelList[i]._memorabiliaCount.ToString();
            }
        }

        private void EnableMemorbiliaPanel()
        {
            _memorbiliaPanel.SetActive(true);
          
            _memorbiliaRewardPanel.SetActive(false);

            DeactiveBingoLineImages();

            DisplayMemorbiliaCollection();

            StartCoroutine(BackToMainScreen());
        }

        private IEnumerator BackToMainScreen()
        {
            yield return new WaitForSeconds(4f);
            _memorbiliaPanel.SetActive(false) ;
            _continentPanel.SetActive(true) ;

            //Reset all the GridButton Answerable
            ResetGridButtons();
        }

        private void ResetGridButtons()
        {
            for(int i=0;i<_gridButtons.Count;i++)
            {
                _gridButtons[i].IsAnswered = false;
                _gridButtons[i].DeactivatedLockedPanel();
            }
        }



        private void EnableGridPanel()
        {
            gridBG.sprite = memorabiliaModelList.Find(x => x._continent == _continent)._gridBG;

            _continentIndex = _dataLoader._continentJsonClassList.FindIndex(x => x._continent == _continent);
            _subContinentIndex = _dataLoader._continentJsonClassList[_continentIndex]._subTypeJsonFileNameList.FindIndex(x => x._subType == _subContinentType);
            _dataLoader.LoadData(_continentIndex, _subContinentIndex);

            _gridIndex = Random.Range(0, _dataLoader.gridData.grids.Count);

            for (int i = 0; i < _dataLoader.gridData.grids[_gridIndex].questions.Length; i++)
            {
                _dataLoader.gridData.grids[_gridIndex].questions[i].questionalreadyShown = false;
            }

            int cIndex = continentGridImageDataList.FindIndex(x => x._continent == _continent);

            int _subCindex = continentGridImageDataList[cIndex].subContinentImageDatas.FindIndex(x => x._subType == _subContinentType);

            for (int i = 0; i < continentGridImageDataList[cIndex].subContinentImageDatas[_subCindex]._gridImageDatas[_gridIndex]._suContinentImage.Count; i++)
            {
                gridUIImages[i].sprite = continentGridImageDataList[cIndex].subContinentImageDatas[_subCindex]._gridImageDatas[_gridIndex]._suContinentImage[i];
            }

            _memorbiliaPanel.SetActive(false);

            _gridPanel.SetActive(true);

            _randomQuestionIndex = Random.Range(0, _dataLoader.gridData.grids[_gridIndex].questions.Length);

            _gridPanelQuestionTMP.text = _dataLoader.gridData.grids[_gridIndex].questions[_randomQuestionIndex].question.ToUpper();

            _dataLoader.gridData.grids[_gridIndex].questions[_randomQuestionIndex].questionalreadyShown = true;

            for (int i = 0; i < _dataLoader.gridData.grids[_gridIndex].questions.Length; i++)
            {
                _gridButtons[i].Answer = _dataLoader.gridData.grids[_gridIndex].questions[i].answer;
                _gridButtons[i].descriptionText = _dataLoader.gridData.grids[_gridIndex].questions[i].oneLiner;
            }

            Debug.LogError("_gridIndex" + _gridIndex);
        }

        public void InstructionsPanelContinueButtonClick()
        {
            _instructionsPanel.SetActive(false);
            _chronosTerraPanel.SetActive(true);
        }

        private void EnableMemorbiliaRewardPanel()
        {
            _memorbiliaRewardPanel.SetActive(true);
            _memorbiliaRewardIM.sprite = memorabiliaModelList.Find(x => x._continent == _continent)._keychain;
            memorabiliaModelList.Find(x => x._continent == _continent)._memorabiliaCount += 1;
            _memorbiliaRewardKeychainTMP.text = _continent.ToString().ToUpper();
            _gridPanel.SetActive(false);

            _inGameAudioSource.clip = _congratsClip;
            _inGameAudioSource.Play();

            StartCoroutine(DelayToEnableMemoriablePanel());
        }

        private IEnumerator DelayToEnableMemoriablePanel()
        {
            yield return new WaitForSeconds(4f);
            EnableMemorbiliaPanel();
        }

        private void DeactiveBingoLineImages()
        {
            firstColumn.SetActive(false);
            secondColumn.SetActive(false);
            thirdColumn.SetActive(false);
            firstRow.SetActive(false);
            secondRow.SetActive(false);
            thirdRow.SetActive(false);
            diagonalLeft.SetActive(false);
            diagonalRight.SetActive(false);
        }

        public void HomeButtonClick()
        {
            _gridPanel.SetActive(false);
            _mainMenuPanel.SetActive(true );
            ResetGridButtons();
        }

        public void InstructionsBackButtonClick()
        {
            _instructionsPanel.SetActive(false);
            _mainMenuPanel.SetActive(true);
        }

        public void ChronousTerraPanelBackButtonClick()
        {
            _chronosTerraPanel.SetActive(false);
            _mainMenuPanel.SetActive(true);
        }

        public void ContinentPanelBackButtonClick()
        {
            _continentPanel.SetActive(false);
            _mainMenuPanel.SetActive(true);
        }

        public void ExitButtonClick()
        {
            Application.Quit();
        }
        #endregion
    }
}

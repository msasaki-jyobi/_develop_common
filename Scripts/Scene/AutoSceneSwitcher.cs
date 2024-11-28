using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class SceneSwitcher : MonoBehaviour
{
    private static SceneSwitcher instance = null;

    [Header("===�ǂݍ��ޗ\��̃V�[����BuildSettings�ɓo�^���Ă�������===")]
    [Header("�ʏ�Load���s�������V�[������o�^")]
    public List<string> sceneListA = new List<string>(); // ���ʂ�LoadScene���s��
    [Header("Additive �ǉ��ǂݍ��݂��s�������V�[����o�^")]
    public List<string> sceneListB = new List<string>(); // Additive��LoadScene���s��

    private string initialScene; // �ŏ��ɊJ�����V�[����
    private List<string> loadedAdditiveScenes = new List<string>(); // Additive�œǂݍ��񂾃V�[�����̃��X�g

    private const int TargetWidth = 1280;
    private const int TargetHeight = 720;

    async void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            initialScene = SceneManager.GetActiveScene().name; // �ŏ��ɊJ�����V�[�����L�^

            // EventSystem�𐶐�
            GameObject eventSystemObj = CreateEventSystem();
            DontDestroyOnLoad(eventSystemObj); // EventSystem��DontDestroy�ɂ���

            // Canvas����������
            Canvas canvas = CreateCanvas();
            canvas.transform.SetParent(this.transform); // Canvas���A�^�b�`�����I�u�W�F�N�g�̎q�ɂ���
            DontDestroyOnLoad(canvas.gameObject); // Canvas��DontDestroy�ɂ���

            await UniTask.Delay(1);
            // �{�^���𐶐�
            CreateButtons(canvas.transform);
        }
    }

    void CreateButtons(Transform parent)
    {
        // �O���b�h���C�A�E�g��ݒ�
        GameObject gridObj = new GameObject("ButtonGrid");
        gridObj.transform.SetParent(parent, false);

        RectTransform gridRect = gridObj.AddComponent<RectTransform>();
        gridRect.anchorMin = new Vector2(0.05f, 0.02f); // �O���b�h����ʉ����ɒ���
        gridRect.anchorMax = new Vector2(0.95f, 0.10f); // ��ʉ���1/12�̗̈�
        gridRect.offsetMin = Vector2.zero;
        gridRect.offsetMax = Vector2.zero;
        gridRect.pivot = new Vector2(0.5f, 0.5f);

        GridLayoutGroup gridLayout = gridObj.AddComponent<GridLayoutGroup>();

        // �{�^���T�C�Y���v�Z
        float buttonSize = AdjustGridLayout(gridLayout);

        for (int i = 0; i < sceneListA.Count; i++)
        {
            string sceneName = sceneListA[i];
            CreateButton(i, sceneName, () => LoadSceneNormal(sceneName), gridObj.transform, buttonSize, Color.gray);
        }

        for (int i = 0; i < sceneListB.Count; i++)
        {
            string sceneName = sceneListB[i];
            CreateButton(i + sceneListA.Count, sceneName, () => LoadSceneAdditive(sceneName), gridObj.transform, buttonSize, Color.blue);
        }

        // �u������Ԃɖ߂��v�{�^����ǉ�
        CreateButton(sceneListA.Count + sceneListB.Count, "Reset to Initial Scene", ResetToInitialScene, gridObj.transform, buttonSize, Color.green);

        DontDestroyOnLoad(gridObj); // Grid��DontDestroy�ɂ���
    }

    void CreateButton(int index, string buttonText, UnityEngine.Events.UnityAction onClickAction, Transform parent, float buttonSize, Color buttonColor)
    {
        GameObject buttonObj = new GameObject($"Button_{index}");
        buttonObj.transform.SetParent(parent, false);

        Button button = buttonObj.AddComponent<Button>();
        button.targetGraphic = buttonObj.AddComponent<Image>();
        button.image.color = buttonColor; // �{�^���̔w�i�F��ݒ�
        button.onClick.AddListener(onClickAction);

        // �e�L�X�g��ǉ�
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);

        Text text = textObj.AddComponent<Text>();
        text.text = InsertLineBreaks(buttonText, 6); // 6�������Ƃɉ��s��}��
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.black;
        text.resizeTextForBestFit = true; // BestFit��L����
        text.resizeTextMinSize = 10; // �ŏ��t�H���g�T�C�Y
        text.resizeTextMaxSize = 100; // �ő�t�H���g�T�C�Y

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        DontDestroyOnLoad(buttonObj); // �e�{�^����DontDestroy�ɂ���
    }

    string InsertLineBreaks(string input, int lineLength)
    {
        if (input.Length <= lineLength)
        {
            return input;
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < input.Length; i += lineLength)
        {
            int length = Mathf.Min(lineLength, input.Length - i);
            sb.Append(input.Substring(i, length));
            if (i + length < input.Length)
            {
                sb.Append("\n"); // ���s��}��
            }
        }
        return sb.ToString();
    }

    void LoadSceneNormal(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    void LoadSceneAdditive(string sceneName)
    {
        if (!loadedAdditiveScenes.Contains(sceneName))
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            loadedAdditiveScenes.Add(sceneName); // �ǂݍ���Additive�V�[�����L�^
        }
    }

    void ResetToInitialScene()
    {
        // ���ׂĂ�Additive�V�[�����A�����[�h
        foreach (string sceneName in loadedAdditiveScenes)
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                SceneManager.UnloadSceneAsync(sceneName);
            }
        }

        loadedAdditiveScenes.Clear(); // ���X�g���N���A

        // �����V�[���ɖ߂�
        SceneManager.LoadScene(initialScene, LoadSceneMode.Single);
    }

    Canvas CreateCanvas()
    {
        GameObject canvasObj = new GameObject("SceneSwitcherCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999; // �őO�ʂɕ\��

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        ConfigureCanvasScaler(scaler);

        canvasObj.AddComponent<GraphicRaycaster>();

        return canvas;
    }

    void ConfigureCanvasScaler(CanvasScaler scaler)
    {
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(TargetWidth, TargetHeight); // ��𑜓x��1280x720�ɐݒ�
        scaler.matchWidthOrHeight = 1f; // �����D��ŃX�P�[��
    }

    GameObject CreateEventSystem()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystemObj = new GameObject("EventSystem");
            EventSystem eventSystem = eventSystemObj.AddComponent<EventSystem>();

            if (UnityEngine.InputSystem.InputSystem.settings != null)
            {
                // �V����Input System���L���ȏꍇ
                eventSystemObj.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
            }
            else
            {
                // �Â�Input Manager���g�p����ꍇ
                eventSystemObj.AddComponent<StandaloneInputModule>();
            }

            return eventSystemObj;
        }
        return null;
    }

    float AdjustGridLayout(GridLayoutGroup gridLayout)
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // �^�[�Q�b�g�𑜓x����Ƀ{�^���̃T�C�Y���v�Z
        float scaleFactor = Mathf.Min(screenWidth / TargetWidth, screenHeight / TargetHeight);
        float buttonSize = 80 * scaleFactor; // ���T�C�Y80�̃X�P�[���K�p

        gridLayout.cellSize = new Vector2(buttonSize, buttonSize);
        gridLayout.spacing = new Vector2(10 * scaleFactor, 10 * scaleFactor); // �X�P�[���ɉ������Ԋu
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayout.constraintCount = 1; // 1�s�̂ݕ\��

        return buttonSize; // �v�Z�����{�^���T�C�Y��Ԃ�
    }
}
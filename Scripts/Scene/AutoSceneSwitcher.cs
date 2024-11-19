using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class SceneSwitcher : MonoBehaviour
{
    private static SceneSwitcher instance = null;

    [Header("===読み込む予定のシーンはBuildSettingsに登録しておくこと===")]
    [Header("通常Loadを行いたいシーン名を登録")]
    public List<string> sceneListA = new List<string>(); // 普通にLoadSceneを行う
    [Header("Additive 追加読み込みを行いたいシーンを登録")]
    public List<string> sceneListB = new List<string>(); // AdditiveでLoadSceneを行う

    private string initialScene; // 最初に開いたシーン名
    private List<string> loadedAdditiveScenes = new List<string>(); // Additiveで読み込んだシーン名のリスト

    private const int TargetWidth = 1280;
    private const int TargetHeight = 720;

    async void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            initialScene = SceneManager.GetActiveScene().name; // 最初に開いたシーンを記録

            // EventSystemを生成
            GameObject eventSystemObj = CreateEventSystem();
            DontDestroyOnLoad(eventSystemObj); // EventSystemもDontDestroyにする

            // Canvasを自動生成
            Canvas canvas = CreateCanvas();
            canvas.transform.SetParent(this.transform); // Canvasをアタッチしたオブジェクトの子にする
            DontDestroyOnLoad(canvas.gameObject); // CanvasをDontDestroyにする

            await UniTask.Delay(1);
            // ボタンを生成
            CreateButtons(canvas.transform);
        }
    }

    void CreateButtons(Transform parent)
    {
        // グリッドレイアウトを設定
        GameObject gridObj = new GameObject("ButtonGrid");
        gridObj.transform.SetParent(parent, false);

        RectTransform gridRect = gridObj.AddComponent<RectTransform>();
        gridRect.anchorMin = new Vector2(0.05f, 0.02f); // グリッドを画面下部に調整
        gridRect.anchorMax = new Vector2(0.95f, 0.10f); // 画面下部1/12の領域
        gridRect.offsetMin = Vector2.zero;
        gridRect.offsetMax = Vector2.zero;
        gridRect.pivot = new Vector2(0.5f, 0.5f);

        GridLayoutGroup gridLayout = gridObj.AddComponent<GridLayoutGroup>();

        // ボタンサイズを計算
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

        // 「初期状態に戻す」ボタンを追加
        CreateButton(sceneListA.Count + sceneListB.Count, "Reset to Initial Scene", ResetToInitialScene, gridObj.transform, buttonSize, Color.green);

        DontDestroyOnLoad(gridObj); // GridもDontDestroyにする
    }

    void CreateButton(int index, string buttonText, UnityEngine.Events.UnityAction onClickAction, Transform parent, float buttonSize, Color buttonColor)
    {
        GameObject buttonObj = new GameObject($"Button_{index}");
        buttonObj.transform.SetParent(parent, false);

        Button button = buttonObj.AddComponent<Button>();
        button.targetGraphic = buttonObj.AddComponent<Image>();
        button.image.color = buttonColor; // ボタンの背景色を設定
        button.onClick.AddListener(onClickAction);

        // テキストを追加
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);

        Text text = textObj.AddComponent<Text>();
        text.text = InsertLineBreaks(buttonText, 6); // 6文字ごとに改行を挿入
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.black;
        text.resizeTextForBestFit = true; // BestFitを有効化
        text.resizeTextMinSize = 10; // 最小フォントサイズ
        text.resizeTextMaxSize = 100; // 最大フォントサイズ

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        DontDestroyOnLoad(buttonObj); // 各ボタンもDontDestroyにする
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
                sb.Append("\n"); // 改行を挿入
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
            loadedAdditiveScenes.Add(sceneName); // 読み込んだAdditiveシーンを記録
        }
    }

    void ResetToInitialScene()
    {
        // すべてのAdditiveシーンをアンロード
        foreach (string sceneName in loadedAdditiveScenes)
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                SceneManager.UnloadSceneAsync(sceneName);
            }
        }

        loadedAdditiveScenes.Clear(); // リストをクリア

        // 初期シーンに戻す
        SceneManager.LoadScene(initialScene, LoadSceneMode.Single);
    }

    Canvas CreateCanvas()
    {
        GameObject canvasObj = new GameObject("SceneSwitcherCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999; // 最前面に表示

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        ConfigureCanvasScaler(scaler);

        canvasObj.AddComponent<GraphicRaycaster>();

        return canvas;
    }

    void ConfigureCanvasScaler(CanvasScaler scaler)
    {
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(TargetWidth, TargetHeight); // 基準解像度を1280x720に設定
        scaler.matchWidthOrHeight = 1f; // 高さ優先でスケール
    }

    GameObject CreateEventSystem()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystemObj = new GameObject("EventSystem");
            EventSystem eventSystem = eventSystemObj.AddComponent<EventSystem>();

            if (UnityEngine.InputSystem.InputSystem.settings != null)
            {
                // 新しいInput Systemが有効な場合
                eventSystemObj.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
            }
            else
            {
                // 古いInput Managerを使用する場合
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

        // ターゲット解像度を基準にボタンのサイズを計算
        float scaleFactor = Mathf.Min(screenWidth / TargetWidth, screenHeight / TargetHeight);
        float buttonSize = 80 * scaleFactor; // 元サイズ80のスケール適用

        gridLayout.cellSize = new Vector2(buttonSize, buttonSize);
        gridLayout.spacing = new Vector2(10 * scaleFactor, 10 * scaleFactor); // スケールに応じた間隔
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayout.constraintCount = 1; // 1行のみ表示

        return buttonSize; // 計算したボタンサイズを返す
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class UnivarsalGravitationController : MonoBehaviour
{
    [Tooltip("Hierarchyビューから,万有引力の対象となるものを\nドラッグ&ドロップでリストに追加してください")]
    [SerializeField]
    public List<Rigidbody> GravityTargets_obj = new List<Rigidbody>();

    [SerializeField]
    public bool VisualSimulation;

    [SerializeField]
    public bool EnglishSave;
    [SerializeField]
    public double coefficient_Exponentiation = -11;
    [HideInInspector]
    public double coefficient;         // 万有引力定数
    void Start()
    {
        for (int i = 0; i <= GravityTargets_obj.Count -1 ; i++){//全てのrigidbodyのUsegravityを外す
            GravityTargets_obj[i].useGravity = false;
        }
    }
    void FixedUpdate()
    { 

        this.coefficient = 6.67408 * System.Math.Pow(10, coefficient_Exponentiation);//本当は-11乗

        for (int i = 0; i <= GravityTargets_obj.Count -1 ;)
        {
            for(int n = 0; n <= GravityTargets_obj.Count -1 ;)
            {
                if (i != n)
                {
                    Vector3 direction = (GravityTargets_obj[i].transform.position - GravityTargets_obj[n].transform.position);

                    // 星までの距離の２乗を取得(同時にdistanceは正になる)
                    double distance = (double)direction.magnitude;
                    distance *= distance;

                    // 万有引力計算
                    double gravity = coefficient * GravityTargets_obj[i].mass * GravityTargets_obj[n].mass / distance;

                    // 力を与える
                    float gravityf = (float)gravity;                                    //double型の万有引力をfloat型に変換
                    GravityTargets_obj[i].AddForce(-gravityf * direction.normalized, ForceMode.Force);//normalizedでgravtyを正規化(方向のみ必要なので)・Forceは質量考慮、継続的
                    
                }
                n++;
            }
            i++;
        }
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(UnivarsalGravitationController))]
public class UnivarsalGravitationControllerEditor : Editor
{
    private UnivarsalGravitationController _target;
    bool English;
    //public Texture TranslateTex ;
    void Awake()
    {
        _target = target as  UnivarsalGravitationController;
        English = _target.EnglishSave;
    }

    public override void OnInspectorGUI()
    {   
        GUIStyle centerbold = new GUIStyle()
        {
        alignment = TextAnchor.MiddleCenter,
        fontStyle = FontStyle.Normal,
        richText  = true
        };
        
        EditorGUI.BeginChangeCheck();

        GUIContent AddAllEnglish = new GUIContent()
        {
        text = "Add all Rigidbodies from the scene",
        tooltip = "Add to the list all GameObjects \nthat have a Rigidbody component in the Scene."
        };
        GUIContent AddAllJapanese = new GUIContent()
        {
        text = "シーン内の全Rigidbodyを追加",
        tooltip = "リストに,シーン内からRigidbodyコンポーネントを\n持つ全てのGameObjectを追加します"
        };

        GUIContent AddAllLanguage = new GUIContent();
        if(English){
            AddAllLanguage = AddAllEnglish;
        }if(!English){
            AddAllLanguage = AddAllJapanese;
        }
        


        if(GUI.Button(new Rect(1,1,120,30),new GUIContent("English / Japanese","Translate English / Japanese")))//English
        {
            if(this.English == false){
                this.English = true;
                _target.EnglishSave = true;
            }
            else/*(this.English == true)*/{
                this.English = false;
                _target.EnglishSave = false;
            }
        }

        if(GUI.Button(new Rect(160,30,205,28),AddAllLanguage)){
            
            Rigidbody[] rblist = FindObjectsOfType<Rigidbody>();
            
            
            _target.GravityTargets_obj = _target.GravityTargets_obj.Union(rblist).ToList();
            IEnumerable<Rigidbody> rblistResult = rblist.Distinct();
        }

        if(English){
            EditorGUILayout.Space(50);
            EditorGUILayout.LabelField("<color=default>--Specify <b>Rigidbodies</b> that are subject to universal gravitation--</color>",centerbold);

            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("GravityTargets_obj"), true);
            serializedObject.ApplyModifiedProperties();


            EditorGUILayout.Space(32);
            EditorGUILayout.HelpBox("If objects seem to be stationary,it is probably because the mass of the 'Rigidbody' is too small and the universal gravitational force is also small, making it difficult to see visually.",MessageType.None);
            EditorGUILayout.Space();
            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("<color=default>--a--d--v--a--n--c--e--d--</color>",centerbold);
            EditorGUILayout.Space(5);

            EditorGUILayout.LabelField("<color=default>The gravitational constant can be increased\nif you want to understand visually.</color>",centerbold);
            EditorGUILayout.Space(5);

            _target.VisualSimulation = EditorGUILayout.Toggle("VisualSimulation", _target.VisualSimulation);


            if (_target.VisualSimulation)
            {
            EditorGUILayout.LabelField("<color=default>[EXPONENT CHANGE]</color>",centerbold);
            _target.coefficient_Exponentiation = (double)EditorGUILayout.Slider("Exponent",(float)_target.coefficient_Exponentiation,-11,3);//_target.coefficient_Exponentiation);
            EditorGUILayout.HelpBox("The original exponent of the gravitational constant is -11 (power).\nThe closer the value is to 3 , the easier it is to see visually.",MessageType.Info);
            }else{
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.LabelField("<color=grey>[EXPONENT CHANGE]</color>",centerbold);
            _target.coefficient_Exponentiation = (double)EditorGUILayout.Slider("Exponent",(float)_target.coefficient_Exponentiation,-11,3);
            EditorGUI.EndDisabledGroup();
            }
        }

        if(!English){//Japanese
            EditorGUILayout.Space(50);
            EditorGUILayout.LabelField("<color=default>--万有引力がかかる対象となる<b>Rigidbody</b>を指定--</color>",centerbold);

            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("GravityTargets_obj"), true);
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space(32);
            EditorGUILayout.HelpBox("物体が止まっているように見えるならば、恐らくRigidbodyの質量が小さすぎて万有引力も小さくなり、視覚的にわかりづらくなっている為です.",MessageType.None);
            EditorGUILayout.Space();
            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("<color=default>--a--d--v--a--n--c--e--d--</color>",centerbold);

            EditorGUILayout.LabelField("<color=default>視覚的に分かりやすくするため万有引力定数を大きくできます</color>",centerbold);

            _target.VisualSimulation = EditorGUILayout.Toggle("VisualSimulation", _target.VisualSimulation);


            if (_target.VisualSimulation)
            {
            EditorGUILayout.LabelField("<color=default>[指数の変更]</color>",centerbold);
            _target.coefficient_Exponentiation = (double)EditorGUILayout.Slider("指数",(float)_target.coefficient_Exponentiation,-11,3);//_target.coefficient_Exponentiation);
            EditorGUILayout.HelpBox("万有引力定数本来の指数は-11(乗)です.\n3 に近づくほど視覚的に分かりやすくなります.",MessageType.Info);
            }else{
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.LabelField("<color=grey>[指数の変更]</color>",centerbold);
            _target.coefficient_Exponentiation = (double)EditorGUILayout.Slider("指数",(float)_target.coefficient_Exponentiation,-11,3);
            EditorGUI.EndDisabledGroup();
            }
        }
    
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(_target);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
#endif

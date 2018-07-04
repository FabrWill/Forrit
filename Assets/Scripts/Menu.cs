using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public Button btn_novo, btn_carregar, btn_opcoes, btn_sair, btn_voltar, btn_salvar;
    public Slider sdl_volume;
    public Toggle tgl_janela;
    public Dropdown dpw_resolucoes, dpw_qualidades;
    public Text txt_volume;
    public Canvas cnv_options, cnv_start;
    public string txt_cena = "Cena do game";

    private string jogo;
    private float volume;
    private int grafico, janela, resolucao;
    private bool tela;
    private Resolution[] resolucoes;

    // funcao para limpar as resolucoes adicionadas e colocar novamente no select
    private void ChecarResolucoes()
    {
        Resolution[] resolucoes = Screen.resolutions;
        dpw_resolucoes.options.Clear();
        for (int i = 0; i < resolucoes.Length; i++)
        {
            dpw_resolucoes.options.Add(new Dropdown.OptionData()
            {
                text = resolucoes[i].width + " x" + resolucoes[i].height
            });
        }
        dpw_resolucoes.captionText.text = "Selecione";
    }

    //funcao para limpar as qualidades e adicionar novamente no select
    private void AjustarQualidade()
    {
        string[] qualidades = QualitySettings.names;
        dpw_qualidades.options.Clear();
        for (int i = 0; i < qualidades.Length; i++)
        {
            dpw_qualidades.options.Add(new Dropdown.OptionData()
            {
                text = qualidades[i]
            });
        }
        dpw_qualidades.captionText.text = "Selecione";
    }

    //muda de canvas
    private void Trocar(bool interruptor)
    {
        cnv_options.gameObject.SetActive(interruptor);
        cnv_start.gameObject.SetActive(!interruptor);
    }

    //salva as preferencias das opções
    private void Salvar() {
        if (tgl_janela.isOn == true)
        {
            janela = 1;
            tela = false;
        } else
        {
            janela = 0;
            tela = true;
        }

        PlayerPrefs.SetFloat("volume", sdl_volume.value);
        PlayerPrefs.SetInt("grafico", dpw_qualidades.value);
        PlayerPrefs.SetInt("janela", janela);
        PlayerPrefs.SetInt("resolucao", dpw_resolucoes.value);
        resolucao = dpw_resolucoes.value;

        volume = PlayerPrefs.GetFloat("volume");
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("grafico"));
        Screen.SetResolution(resolucoes[resolucao].width, resolucoes[resolucao].height, tela);

    }

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        resolucoes = Screen.resolutions;
        
    }

    // Use this for initialization
    void Start () {

        Trocar(false);
        ChecarResolucoes();
        AjustarQualidade();

        if (PlayerPrefs.HasKey("resolucao"))
        {
            int numResoluc = PlayerPrefs.GetInt("resolucao");
            if (resolucoes.Length <= numResoluc)
            {
                PlayerPrefs.DeleteKey("resolucao");
            }
        }
        //
        txt_cena = SceneManager.GetActiveScene().name;
        Cursor.visible = true;
        Time.timeScale = 1;
        //
        sdl_volume.minValue = 0;
        sdl_volume.maxValue = 1;

        //=============== SAVES===========//
        if (PlayerPrefs.HasKey("volume"))
        {
            volume = PlayerPrefs.GetFloat("volume");
            sdl_volume.value = volume;
        }
        else
        {
            PlayerPrefs.SetFloat("volume", 1);
            sdl_volume.value = 1;
        }
        //=============MODO JANELA===========//
        if (PlayerPrefs.HasKey("modoJanela"))
        {
            janela = PlayerPrefs.GetInt("modoJanela");
            if (janela == 1)
            {
                Screen.fullScreen = false;
                tgl_janela.isOn = true;
            }
            else
            {
                Screen.fullScreen = true;
                tgl_janela.isOn = false;
            }
        }
        else
        {
            janela = 0;
            PlayerPrefs.SetInt("modoJanela", janela);
            tgl_janela.isOn = false;
            Screen.fullScreen = true;
        }
        //========RESOLUCOES========//
        if (janela == 1)
        {
            tela = false;
        }
        else
        {
            tela = true;
        }
        if (PlayerPrefs.HasKey("resolucao"))
        {
            resolucao = PlayerPrefs.GetInt("resolucao");
            Screen.SetResolution(resolucoes[resolucao].width, resolucoes[resolucao].height, tela);
            dpw_resolucoes.value = resolucao;
        }
        else
        {
            resolucao = (resolucoes.Length - 1);
            Screen.SetResolution(resolucoes[resolucao].width, resolucoes[resolucao].height, tela);
            PlayerPrefs.SetInt("resolucao", resolucao);
            dpw_resolucoes.value = resolucao;
        }
        //=========QUALIDADES=========//
        if (PlayerPrefs.HasKey("graficos"))
        {
            grafico = PlayerPrefs.GetInt("graficos");
            QualitySettings.SetQualityLevel(grafico);
            dpw_qualidades.value = grafico;
        }
        else
        {
            QualitySettings.SetQualityLevel((QualitySettings.names.Length - 1));
            grafico = (QualitySettings.names.Length - 1);
            PlayerPrefs.SetInt("graficos", grafico);
            dpw_qualidades.value = grafico;
        }

        // =========SETAR BOTOES==========//
        btn_novo.onClick = new Button.ButtonClickedEvent();
        btn_opcoes.onClick = new Button.ButtonClickedEvent();
        btn_sair.onClick = new Button.ButtonClickedEvent();
        btn_voltar.onClick = new Button.ButtonClickedEvent();
        btn_salvar.onClick = new Button.ButtonClickedEvent();
        btn_novo.onClick.AddListener(() => Jogar());
        btn_opcoes.onClick.AddListener(() => Trocar(true));
        btn_sair.onClick.AddListener(() => Sair());
        btn_voltar.onClick.AddListener(() => Trocar(false));
        btn_salvar.onClick.AddListener(() => Salvar());
    }

    private void Jogar()
    {
        SceneManager.LoadScene(txt_cena);
    }
    private void Sair()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update () {
        if (SceneManager.GetActiveScene().name != txt_cena)
        {
            AudioListener.volume = volume;
            Destroy(gameObject);
        }
    }
}

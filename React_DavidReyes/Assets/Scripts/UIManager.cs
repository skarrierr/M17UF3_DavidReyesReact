using UnityEngine;
using UnityEngine.UI;
using TMPro;                  // Si usas TextMeshPro InputField
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public Button hostButton;
    public Button joinButton;
    public InputField joinCodeInput;   // Si usas TextMeshPro
                                           // public InputField joinCodeInput;    // Si usas la UI cl�sica

    [Header("Relay Manager")]
    public RelayManager relayManager;      // Arr�stralo desde el inspector

    void Start()
    {
        hostButton.onClick.AddListener(OnHostClicked);
        joinButton.onClick.AddListener(OnJoinClicked);
    }

    void OnHostClicked()
    {
        // Deshabilitar botones para evitar clicks m�ltiples
        hostButton.interactable = false;
        joinButton.interactable = false;
        StartCoroutine(StartHostRoutine());
    }

    IEnumerator StartHostRoutine()
    {
        // Llamamos al m�todo async y esperamos
        bool done = false;
        relayManager.StartHost();
        // Podemos esperar un frame o hasta que RelayManager imprima el c�digo
        yield return null;
    }

    void OnJoinClicked()
    {
        string code = joinCodeInput.text.Trim();
        if (string.IsNullOrEmpty(code))
        {
            Debug.LogWarning("Introduce un c�digo v�lido");
            return;
        }
        hostButton.interactable = false;
        joinButton.interactable = false;
        relayManager.JoinRelay(code);
    }
}
    
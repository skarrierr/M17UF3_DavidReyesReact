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
                                           // public InputField joinCodeInput;    // Si usas la UI clásica

    [Header("Relay Manager")]
    public RelayManager relayManager;      // Arrástralo desde el inspector

    void Start()
    {
        hostButton.onClick.AddListener(OnHostClicked);
        joinButton.onClick.AddListener(OnJoinClicked);
    }

    void OnHostClicked()
    {
        // Deshabilitar botones para evitar clicks múltiples
        hostButton.interactable = false;
        joinButton.interactable = false;
        StartCoroutine(StartHostRoutine());
    }

    IEnumerator StartHostRoutine()
    {
        // Llamamos al método async y esperamos
        bool done = false;
        relayManager.StartHost();
        // Podemos esperar un frame o hasta que RelayManager imprima el código
        yield return null;
    }

    void OnJoinClicked()
    {
        string code = joinCodeInput.text.Trim();
        if (string.IsNullOrEmpty(code))
        {
            Debug.LogWarning("Introduce un código válido");
            return;
        }
        hostButton.interactable = false;
        joinButton.interactable = false;
        relayManager.JoinRelay(code);
    }
}
    
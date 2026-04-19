using UnityEngine;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.SceneManagement;

public class FireBase : MonoBehaviour
{
    [Header("Scene Navigation")]
    public string menuSceneName = "MainMenu";

    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject signupPanel;

    [Header("Login Fields")]
    public TMP_InputField loginEmail;
    public TMP_InputField loginPassword;

    [Header("Signup Fields")]
    public TMP_InputField signupEmail;
    public TMP_InputField signupPassword;
    public TMP_InputField signupCPassword;
    public TMP_InputField signupUserName;

    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
    }

    public void OpenSignupPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(true);
    }

    public async void OnForgetPasswordButtonClicked()
    {
        if (string.IsNullOrEmpty(loginEmail.text))
        {
            Debug.LogWarning("Please enter your email into the Login Email field first to receive a password reset link.");
            return;
        }

        if (DataManagementFacade.Instance == null)
        {
            Debug.LogError("CRITICAL ERROR");
            return;
        }

        try
        {
            await DataManagementFacade.Instance.AuthService.SendPasswordResetEmail(loginEmail.text);
            Debug.Log($"Password reset email successfully sent to: {loginEmail.text}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to send password reset email: {e.Message}");
        }
    }
    public async void OnLoginButtonClicked()
    {
        if (string.IsNullOrEmpty(loginEmail.text) || string.IsNullOrEmpty(loginPassword.text))
        {
            Debug.LogWarning("Email or password cannot be empty.");
            return;
        }

        if (DataManagementFacade.Instance == null)
        {
            Debug.LogError("CRITICAL ERROR");
            return;
        }
        try
        {
            string userId = await DataManagementFacade.Instance.AuthService.Login(
                loginEmail.text,
                loginPassword.text
            );

            Debug.Log($"Login successful{userId}");
            SceneManager.LoadScene(menuSceneName);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Login failed: {e.Message}");
        }
    }
    public async void OnSignupButtonClicked()
    {
        if (string.IsNullOrEmpty(signupEmail.text) ||
            string.IsNullOrEmpty(signupPassword.text) ||
            string.IsNullOrEmpty(signupCPassword.text))
        {
            Debug.LogWarning("All fields must be filled.");
            return;
        }

        if (signupPassword.text != signupCPassword.text)
        {
            Debug.LogError("Passwords do not match!");
            return;
        }

        if (DataManagementFacade.Instance == null)
        {
            Debug.LogError("CRITICAL ERROR");
            return;
        }

        try
        {
            string userId = await DataManagementFacade.Instance.AuthService.Register(
                signupEmail.text,
                signupPassword.text,
                signupUserName.text
            );

            Debug.Log($"Signup successful{userId}");
            OpenLoginPanel();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Register failed{e.Message}");
        }
    }
}
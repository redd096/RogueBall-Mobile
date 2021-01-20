namespace redd096
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("redd096/MonoBehaviours/UI Manager")]
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject pauseMenu = default;

        [Header("Parry")]
        [SerializeField] GameObject parryObjectToActivate = default;
        [SerializeField] float timeBeforeDeactivateParry = 0.7f;
        Coroutine deactivateParryUICoroutine;

        void Start()
        {
            PauseMenu(false);

            //by default, deactivate parry
            parryObjectToActivate.SetActive(false);
        }

        public void PauseMenu(bool active)
        {
            if (pauseMenu == null)
                return;

            //active or deactive pause menu
            pauseMenu.SetActive(active);
        }

        #region parry

        public void ActiveParryUI()
        {
            //activate
            parryObjectToActivate.SetActive(true);

            //then start timer for deactivate
            if (deactivateParryUICoroutine != null)
                StopCoroutine(deactivateParryUICoroutine);

            deactivateParryUICoroutine = StartCoroutine(DeactivateParryUI());
        }

        IEnumerator DeactivateParryUI()
        {
            //wait, then deactivate parry
            yield return new WaitForSeconds(timeBeforeDeactivateParry);

            parryObjectToActivate.SetActive(false);
        }

        #endregion

    }
}
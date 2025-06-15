using AMAZON.Control;
using AMAZON.Core;
using UnityEngine;
using UnityEngine.Playables;

public class CinematicsControlRemover : MonoBehaviour
{
    [SerializeField] private PlayableDirector _playableDirector;

    private void OnEnable()
    {
        _playableDirector.played += OnDisableControls;
        _playableDirector.stopped += OnEnableControls;
    }

    private void OnDisable()
    {
        _playableDirector.played -= OnDisableControls;
        _playableDirector.stopped -= OnEnableControls;
    }

    private void OnDisableControls(PlayableDirector pd)
    {
        PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ActionScheduler playerActionScheduler = playerController.GetComponent<ActionScheduler>();

        playerActionScheduler.CancelCurrentAction();
        playerController.enabled = false;
    }

    private void OnEnableControls(PlayableDirector pd)
    {
        PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        playerController.enabled = true;
    }
}
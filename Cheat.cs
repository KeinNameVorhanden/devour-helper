using UnityEngine;
using System;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using BehaviorDesigner.Runtime;
using Opsive.Shared.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ExampleAssembly
{
    public class Cheat : MonoBehaviour
    {
        private string sceneName = "";
        private bool isDevour = false;

        private double get_dist(float a_x, float a_y, float a_z, float b_x, float b_y, float b_z)
        {
            return Math.Sqrt(Math.Pow(a_x - b_x, 2) + Math.Pow(a_y - b_y, 2) + Math.Pow(a_z - b_z, 2));
        }

        private void drawCheatInfo(float x, float y, string txt)
        {
            GUI.color = Color.black;
            GUI.Label(new Rect(new Vector2(x + 1, y + 1), new Vector2(100f, 100f)), txt);
            GUI.color = Color.white;
            GUI.Label(new Rect(new Vector2(x, y), new Vector2(100f, 100f)), txt);
        }

        // Runs once.
        private void Start()
        {
            Utils.CreateConsole();

            new Thread(() =>
            {
                while (true)
                {
                    //
                    Console.Clear();
                    Console.WriteLine("Current Scene: " + sceneName);
                    Thread.Sleep(30000);
                }
            }).Start();
        }

        // Runs every frame.
        private void Update()
        {
            sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "Devour" || sceneName == "Menu")
                isDevour = true;
            else
                isDevour = false;
        }

        // Runs every frame, only use it for drawing using Unity's UI.
        private void OnGUI()
        {
            GUI.color = Color.white;

            if (sceneName == "Menu")
                drawCheatInfo(8f, 5f, "Menu");
            else
                drawCheatInfo(8f, 5f, isDevour ? "Farmhouse" : "Asylum");

            // Ghetto Player ESP
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                Vector3 player_vec = Camera.main.WorldToScreenPoint(player.transform.position);
                if (player_vec.z > 0f)
                {
                    player_vec.y = UnityEngine.Screen.height - (player_vec.y + 1f);
                    drawCheatInfo(player_vec.x, player_vec.y, "Player");
                }
            }

            // Goat/Rat ESP (functional)
            foreach (GameObject goat in GameObject.FindGameObjectsWithTag("Goat"))
            {
                Vector3 goat_vec = Camera.main.WorldToScreenPoint(goat.transform.position);
                if (goat_vec.z > 0f)
                {
                    goat_vec.y = UnityEngine.Screen.height - (goat_vec.y + 1f);
                    drawCheatInfo(goat_vec.x, goat_vec.y, isDevour ? "Goat" : "Rat");
                }
            }

            // AI ESP (functional)
            foreach (GameObject ai in GameObject.FindGameObjectsWithTag("AI"))
            {
                Vector3 ai_vec = Camera.main.WorldToScreenPoint(ai.transform.position);
                if (ai_vec.z > 0f)
                {
                    ai_vec.y = UnityEngine.Screen.height - (ai_vec.y + 1f);
                    drawCheatInfo(ai_vec.x, ai_vec.y, isDevour ? "Demon" : "Inmate");
                }
            }

            // Anna/Molly (Azazel) ESP (functional)
            foreach (GameObject azazel in GameObject.FindGameObjectsWithTag("Azazel"))
            {
                Vector3 azazel_vec = Camera.main.WorldToScreenPoint(azazel.transform.position);
                if (azazel_vec.z > 0f)
                {
                    azazel_vec.y = UnityEngine.Screen.height - (azazel_vec.y + 1f);
                    drawCheatInfo(azazel_vec.x, azazel_vec.y, isDevour ? "Anna" : "Molly");
                }
            }

            // Hay/Rotten Flesh ESP (functional)
            foreach (GameObject hay in GameObject.FindGameObjectsWithTag("Hay"))
            {
                Vector3 hay_vec = Camera.main.WorldToScreenPoint(hay.transform.position);
                if (hay_vec.z > 0f)
                {
                    hay_vec.y = UnityEngine.Screen.height - (hay_vec.y + 1f);
                    drawCheatInfo(hay_vec.x, hay_vec.y, isDevour ? "Hay" : "Flesh");
                }
            }

            /* First Aid ESP (untested)
            foreach (SurvivalReviveInteractable survivalReviveInteractable in UnityEngine.Object.FindObjectsOfType<SurvivalReviveInteractable>())
            {
                if (survivalReviveInteractable.name.ToLower().IndexOf("firstaid") != -1)
                {
                    Vector3 med_vec = Camera.main.WorldToScreenPoint(survivalReviveInteractable.transform.position);
                    if (med_vec.z > 0f)
                    {
                        med_vec.y = UnityEngine.Screen.height - (med_vec.y + 1f);
                        drawCheatInfo(med_vec.x, med_vec.y,"Med");
                    }
                }
            }*/

            //* First Aid ESP (functional)
            foreach (GameObject med in GameObject.FindGameObjectsWithTag("FirstAid"))
            {
                Vector3 med_vec = Camera.main.WorldToScreenPoint(med.transform.position);
                if (med_vec.z > 0f)
                {
                    med_vec.y = UnityEngine.Screen.height - (med_vec.y + 1f);
                    drawCheatInfo(med_vec.x, med_vec.y, "Med");
                }
            }//*/

            // Gasoline ESP (functional)
            foreach (SurvivalInteractable survivalInteractable in UnityEngine.Object.FindObjectsOfType<SurvivalInteractable>())
            {
                if (survivalInteractable.name.ToLower().IndexOf("gasoline") != -1)
                {
                    Vector3 gas_vec = Camera.main.WorldToScreenPoint(survivalInteractable.transform.position);
                    if (gas_vec.z > 0f)
                    {
                        gas_vec.y = UnityEngine.Screen.height - (gas_vec.y + 1f);
                        drawCheatInfo(gas_vec.x, gas_vec.y, "Fuel");
                    }
                }
            }

            // Rose ESP (functional)
            foreach (CollectableInteractable collectableInteractable in UnityEngine.Object.FindObjectsOfType<CollectableInteractable>())
            {
                if (collectableInteractable.name.ToLower().IndexOf("rose") != -1)
                {
                    Vector3 rose_vec = Camera.main.WorldToScreenPoint(collectableInteractable.transform.position);
                    if (rose_vec.z > 0f)
                    {
                        rose_vec.y = UnityEngine.Screen.height - (rose_vec.y + 1f);
                        drawCheatInfo(rose_vec.x, rose_vec.y, "Rose");
                    }
                }
            }

            // Key ESP (functional)
            foreach (KeyBehaviour keyBehaviour in UnityEngine.Object.FindObjectsOfType<KeyBehaviour>())
            {
                if (keyBehaviour.name.ToLower().IndexOf("key") != -1)
                {
                    Vector3 key_vec = Camera.main.WorldToScreenPoint(keyBehaviour.transform.position);
                    if (key_vec.z > 0f)
                    {
                        key_vec.y = UnityEngine.Screen.height - (key_vec.y + 1f);
                        drawCheatInfo(key_vec.x, key_vec.y, "Key");
                    }
                }
            }
        }
    }
}

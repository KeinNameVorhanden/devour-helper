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
        private bool drawInfo = true,
            key_esp = true,
            collectable_esp = true,
            useable_esp = true,
            med_esp = true,
            feedable_esp = true,
            boss_esp = true,
            ais_esp = true,
            killable_esp = true;

        private double get_dist(float a_x, float a_y, float a_z, float b_x, float b_y, float b_z)
        {
            return Math.Sqrt(Math.Pow(a_x - b_x, 2) + Math.Pow(a_y - b_y, 2) + Math.Pow(a_z - b_z, 2));
        }

        private void drawCheatInfo(float x, float y, string txt)
        {
            //GUI.color = Color.black;
            //GUI.Label(new Rect(new Vector2(x + 1, y + 1), new Vector2(100f, 100f)), txt);
            //GUI.color = Color.white;
            GUI.Label(new Rect(new Vector2(x, y), new Vector2(255f, 500f)), txt);
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
                    StartCoroutine(updateEnums());
                    sceneName = SceneManager.GetActiveScene().name;

                    if (sceneName == "Devour" || sceneName == "Menu")
                        isDevour = true;
                    else
                        isDevour = false;

                    Console.WriteLine("Current Scene: " + sceneName);
                    Thread.Sleep(5000);
                }
            }).Start();
        }

        // Runs every frame.
        private void Update()
        {
            StartCoroutine(KeyHandler());
        }

        // Runs every frame, only use it for drawing using Unity's UI.
        private void OnGUI()
        {
            GUI.color = Color.white;

            if (drawInfo)
            {
                drawCheatInfo(10f, 250f, "<b>Enabled: " + (drawInfo ? "<color=green>" : "<color=red>") + drawInfo + 
                    "</color>\n | " + (isDevour ? "Fuel" : "Fuse") + " ESP: " + (useable_esp ? "<color=green>" : "<color=red>") + useable_esp +
                    "</color>\n | " + (isDevour ? "Hay" : "Flesh") + " ESP: " + (feedable_esp ? "<color=green>" : "<color=red>") + feedable_esp +
                    "</color>\n | First Aid ESP: " + (med_esp ? "<color=green>" : "<color=red>") + med_esp +
                    "</color>\n | " + (isDevour ? "Anna" : "Molly") + " ESP: " + (boss_esp ? "<color=green>" : "<color=red>") + boss_esp +
                    "</color>\n | " + (isDevour ? "Demon" : "Inmate") + " ESP: " + (ais_esp ? "<color=green>" : "<color=red>") + ais_esp +
                    "</color>\n | " + (isDevour ? "Goat" : "Rat") + " ESP: " + (killable_esp ? "<color=green>" : "<color=red>") + killable_esp +
                    "</color>\n | Key ESP: " + (key_esp ? "<color=green>" : "<color=red>") + key_esp +
                    "</color>\n | " + (isDevour ? "Rose" : "Cloth") + " ESP: " + (collectable_esp ? "<color=green>" : "<color=red>") + collectable_esp +
                "</color></b>");

                if (sceneName == "Menu")
                    drawCheatInfo(10f, 7f, "<b>Menu</b>");
                else
                    drawCheatInfo(10f, 7f, "<b><color=#ffffffff>" + (isDevour ? "Farmhouse" : "Asylum") + "</color></b>");
            }

            /*/ Ghetto Player ESP
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                Vector3 player_vec = Camera.main.WorldToScreenPoint(player.transform.position);
                if (player_vec.z > 0f)
                {
                    player_vec.y = UnityEngine.Screen.height - (player_vec.y + 1f);
                    drawCheatInfo(player_vec.x, player_vec.y, "Player");
                }
            }*/

            // Gasoline ESP (functional)
            if (useable_esp)
                if (useables.Count > 0)
                    foreach (SurvivalInteractable useable in useables)
                    {
                        if (useable.name.ToLower().IndexOf("gasoline") != -1 || useable.name.ToLower().IndexOf("fuse") != -1)
                        {
                            Vector3 gas_vec = Camera.main.WorldToScreenPoint(useable.transform.position);
                            if (gas_vec != Vector3.zero && gas_vec != null)
                            {
                                if (gas_vec.z > 0f)
                                {
                                    gas_vec.y = UnityEngine.Screen.height - (gas_vec.y + 1f);
                                    drawCheatInfo(gas_vec.x, gas_vec.y, "<b><color=#32e6e0ff>" + (isDevour ? "Fuel" : "Fuse") + "</color></b>");
                                }
                            }
                        }
                    }
                       
            // Hay/Rotten Flesh ESP (functional)
            if (feedable_esp)
                if (feedables.Count > 0)
                    foreach (GameObject feedable in feedables)
                    {
                        Vector3 feedable_vec = Camera.main.WorldToScreenPoint(feedable.transform.position);
                        if (feedable_vec != Vector3.zero && feedable_vec != null)
                        {
                            if (feedable_vec.z > 0f)
                            {
                                feedable_vec.y = UnityEngine.Screen.height - (feedable_vec.y + 1f);
                                drawCheatInfo(feedable_vec.x, feedable_vec.y, "<b><color=#e69b32ff>" + (isDevour ? "Hay" : "Flesh") + "</color></b>");
                            }
                        }
                    }

            //* First Aid ESP (functional)
            if (med_esp)
                if (meds.Count > 0)
                    foreach (GameObject med in meds)
                    {
                        Vector3 med_vec = Camera.main.WorldToScreenPoint(med.transform.position);
                        if (med_vec != Vector3.zero && med_vec != null)
                        {
                            if (med_vec.z > 0f)
                            {
                                med_vec.y = UnityEngine.Screen.height - (med_vec.y + 1f);
                                drawCheatInfo(med_vec.x, med_vec.y, "<b><color=#32e635ff>Med</color></b>");
                            }
                        }
                    }//*/

            // Rose ESP (functional)
            if (collectable_esp)
                if (collectables.Count > 0)
                    foreach (CollectableInteractable collectable in collectables)
                    {
                        Vector3 collectable_vec = Camera.main.WorldToScreenPoint(collectable.transform.position);
                        if (collectable_vec != Vector3.zero && collectable_vec != null)
                        {
                            if (collectable_vec.z > 0f)
                            {
                                collectable_vec.y = UnityEngine.Screen.height - (collectable_vec.y + 1f);
                                drawCheatInfo(collectable_vec.x, collectable_vec.y, "<b><color=#e63289ff>" + (isDevour ? "Rose" : "Cloth") + "</color></b>");
                            }
                        }
                    }

            // Key ESP (functional)
            if (key_esp)
                if (keys.Count > 0)
                    foreach (KeyBehaviour key in keys)
                    {
                        if (key.name.ToLower().IndexOf("key") != -1)
                        {
                            Vector3 key_vec = Camera.main.WorldToScreenPoint(key.transform.position);
                            if (key_vec != Vector3.zero && key_vec != null)
                            {
                                if (key_vec.z > 0f)
                                {
                                    key_vec.y = UnityEngine.Screen.height - (key_vec.y + 1f);
                                    drawCheatInfo(key_vec.x, key_vec.y, "<b><color=#343debff>Key</color></b>");
                                }
                            }
                        }
                    }

            // Goat/Rat ESP (functional)
            if (killable_esp)
                if (killables.Count > 0)
                    foreach (GameObject killable in killables)
                    {
                        Vector3 killable_vec = Camera.main.WorldToScreenPoint(killable.transform.position);
                        if (killable_vec != Vector3.zero && killable_vec != null)
                        {
                            if (killable_vec.z > 0f)
                            {
                                killable_vec.y = UnityEngine.Screen.height - (killable_vec.y + 1f);
                                drawCheatInfo(killable_vec.x, killable_vec.y, "<b><color=#7732e6ff>" + (isDevour ? "Goat" : "Rat") + "</color></b>");
                            }
                        }
                    }

            // Anna/Molly (Azazel) ESP (functional)
            if (boss_esp)
            {
                if (boss && boss != null)
                {
                    Vector3 boss_vec = Camera.main.WorldToScreenPoint(boss.transform.position);
                    if (boss_vec != Vector3.zero && boss_vec != null)
                    {
                        if (boss_vec != Vector3.zero && boss_vec != null)
                        {
                            if (boss_vec.z > 0f)
                            {
                                boss_vec.y = UnityEngine.Screen.height - (boss_vec.y + 1f);
                                drawCheatInfo(boss_vec.x, boss_vec.y, "<b><color=#e63e32ff>" + (isDevour ? "Anna" : "Molly") + "</color></b>");
                            }
                        }
                    }
                }
            }

            // AI ESP (functional)
            if (ais_esp)
                if (ais.Count > 0)
                    foreach (GameObject ai in ais)
                    {   
                        Vector3 ai_vec = Camera.main.WorldToScreenPoint(ai.transform.position);
                        if (ai_vec != Vector3.zero && ai_vec != null)
                        {
                            if (ai_vec.z > 0f)
                            {
                                ai_vec.y = UnityEngine.Screen.height - (ai_vec.y + 1f);
                                drawCheatInfo(ai_vec.x, ai_vec.y, "<b><color=#dae632ff>" + (isDevour ? "Demon" : "Inmate") + "</color></b>");
                            }
                        }
                }
        }

        IEnumerator KeyHandler()
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
                useable_esp = !useable_esp;

            if (Input.GetKeyDown(KeyCode.Keypad2))
                feedable_esp = !feedable_esp;

            if (Input.GetKeyDown(KeyCode.Keypad3))
                med_esp = !med_esp;

            if (Input.GetKeyDown(KeyCode.Keypad4))
                boss_esp = !boss_esp;

            if (Input.GetKeyDown(KeyCode.Keypad5))
                ais_esp = !ais_esp;

            if (Input.GetKeyDown(KeyCode.Keypad6))
                killable_esp = !killable_esp;

            if (Input.GetKeyDown(KeyCode.Keypad7))
                key_esp = !key_esp;

            if (Input.GetKeyDown(KeyCode.Keypad8))
                collectable_esp = !collectable_esp;

            if (Input.GetKeyDown(KeyCode.Keypad0))
                drawInfo = !drawInfo;

            yield return new WaitForEndOfFrame();
        }

        IEnumerator updateEnums()
        {
            keys = FindObjectsOfType<KeyBehaviour>().ToList();
            yield return new WaitForSeconds(0.15f);

            collectables = FindObjectsOfType<CollectableInteractable>().ToList();
            yield return new WaitForSeconds(0.15f);

            useables = FindObjectsOfType<SurvivalInteractable>().ToList();
            yield return new WaitForSeconds(0.15f);

            meds = GameObject.FindGameObjectsWithTag("FirstAid").ToList();
            yield return new WaitForSeconds(0.15f);

            feedables = GameObject.FindGameObjectsWithTag("Hay").ToList();
            yield return new WaitForSeconds(0.15f);

            boss = GameObject.FindGameObjectWithTag("Azazel");
            yield return new WaitForSeconds(0.15f);

            ais = GameObject.FindGameObjectsWithTag("AI").ToList();
            yield return new WaitForSeconds(0.15f);

            killables = GameObject.FindGameObjectsWithTag("Goat").ToList();
            yield return new WaitForSeconds(0.15f);

            yield return null;
        }

        public static List<KeyBehaviour> keys;
        public static List<CollectableInteractable> collectables;
        public static List<SurvivalInteractable> useables;
        public static List<GameObject> meds;
        public static List<GameObject> feedables;
        public static GameObject boss;
        public static List<GameObject> ais;
        public static List<GameObject> killables;
    }
}

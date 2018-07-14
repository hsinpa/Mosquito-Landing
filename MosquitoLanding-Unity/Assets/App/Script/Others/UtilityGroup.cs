using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;
using System;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Net;

namespace Utility
{
    public class UtilityGroup
    {

        /// <summary>
        ///  Load single sprite from multiple mode
        /// </summary>
        /// <param name="spriteArray"></param>
        /// <param name="spriteName"></param>
        /// <returns></returns>
        public static Sprite LoadSpriteFromMulti(Sprite[] spriteArray, string spriteName)
        {
            foreach (Sprite s in spriteArray)
            {

                if (s.name == spriteName) return s;
            }
            return null;
        }

        /// <summary>
        /// Clear every child gameobject
        /// </summary>
        /// <param name="parent"></param>
        public static void ClearChildObject(Transform parent)
        {
            foreach (Transform t in parent)
            {
                GameObject.Destroy(t.gameObject);
            }
        }

        public static void DeleteObject(GameObject p_object)
        {
            if (Application.isPlaying) GameObject.Destroy(p_object);
            if (Application.isEditor) GameObject.DestroyImmediate(p_object);
        }


        /// <summary>
        ///  Insert gameobject to parent
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public static GameObject CreateObjectToParent(Transform parent, GameObject prefab)
        {
            GameObject item = GameObject.Instantiate(prefab);
            item.transform.SetParent(parent);
            item.transform.localScale = Vector3.one;
            item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, 1);
            item.transform.localPosition = new Vector3(0, 0, 1);
            return item;
        }

        public static GameObject FindObject(GameObject parent, string name)
        {
            Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in trs)
            {
                if (t.name == name)
                {
                    return t.gameObject;
                }
            }
            return null;
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }



        /// <summary>
        /// Rolls the dice, only return 1 or 0.
        /// </summary>
        /// <returns>The dice.</returns>
        public static int RollDice()
        {
            return Mathf.RoundToInt(UnityEngine.Random.Range(0, 2));
        }

        /// <summary>
        /// Possibilities the match.
        /// </summary>
        /// <returns><c>true</c>, if match was possibilityed, <c>false</c> otherwise.</returns>
        public static bool PercentageGame(float rate)
        {
            float testValue = UnityEngine.Random.Range(0f, 1f);
            return (rate >= testValue) ? true : false;
        }

        public static T PercentageTurntable<T>(T[] p_group, float[] percent_array)
        {
            float percent = UnityEngine.Random.Range(0f, 100f);
            float max = 100;

            for (int i = 0; i < percent_array.Length; i++)
            {
                float newMax = max - percent_array[i];
                if (max >= percent && newMax <= percent) return p_group[i];

                max = newMax;
            }
            return default(T);
        }

        public static T PercentageTurntable<T>(T[] p_group, int[] percent_array)
        {
            float[] convertFloat = System.Array.ConvertAll(percent_array, s => (float)s);
            return PercentageTurntable<T>(p_group, convertFloat);
        }


        public static void TabHandler(Button b, Transform buttonGroup, bool changeFontColor = true)
        {
            foreach (Transform child in buttonGroup)
            {
                if (changeFontColor) child.Find("field").GetComponent<Text>().color = Color.white;
                child.Find("background").GetComponent<Image>().enabled = false;
            }

            b.transform.Find("background").GetComponent<Image>().enabled = true;
            if (changeFontColor) b.transform.Find("field").GetComponent<Text>().color = Color.black;
        }


        public static void FramePageHandler(CanvasGroup canvas, Transform buttonGroup)
        {
            foreach (Transform child in buttonGroup)
            {

                CanvasGroup closeCanvas = child.GetComponent<CanvasGroup>();
                if (closeCanvas == null) continue;

                closeCanvas.alpha = 0;
                closeCanvas.interactable = false;
                closeCanvas.blocksRaycasts = false;
            }
            canvas.alpha = 1;
            canvas.interactable = true;
            canvas.blocksRaycasts = true;
        }

        public static T[] RandomizeArray<T>(T[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int r = UnityEngine.Random.Range(0, i);
                T tmp = array[i];
                array[i] = array[r];
                array[r] = tmp;
            }
            return array;
        }

        public static int RoundFiveUp(int value)
        {
            return Mathf.CeilToInt(value / 5) * 5;
        }

        /// <summary>
        /// Round to nearest ceiling 10
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int RoundUp(float value)
        {
            return 10 * ((Mathf.RoundToInt(value) + 9) / 10);
        }

        public static int RoundHundredUp(float value)
        {
            return 100 * ((Mathf.RoundToInt(value) + 99) / 100);
        }

        /// <summary>
        /// Round to nearest floor 10
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int RoundDown(float value)
        {
            return 10 * (Mathf.RoundToInt(value) / 10);
        }

        public static int ManipulateValue(int target, int effectValue, bool isIncremental)
        {
            if (isIncremental) return target += effectValue;
            return target = effectValue;
        }

        public static string StringReplace(string p_text, params string[] p_values)
        {
            for (int i = 0; i < p_values.Length; i++)
            {
                p_text = System.Text.RegularExpressions.Regex.Replace(p_text, ":" + i, p_values[i]);
            }
            return p_text;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static void PrefSave(string p_key, string p_data)
        {
            PlayerPrefs.SetString(p_key, Base64Encode(p_data));
        }

        public static string PrefGet(string p_key, string p_default = "")
        {
            if (PlayerPrefs.HasKey(p_key))
            {
                return Base64Decode(PlayerPrefs.GetString(p_key));
            }
            return p_default;
        }


        public static JSONObject AnalyzeTextOperator(string p_operator, List<JSONObject> p_score_list)
        {
            switch (p_operator)
            {
                case "Max":
                    return p_score_list.OrderByDescending(x => x.GetField("value").n).First();

                case "Min":
                    return p_score_list.OrderBy(x => x.GetField("value").n).First();
            }
            return p_score_list[0];
        }


        public static string GetJSONValueAsString(JSONObject p_json, string p_key)
        {

            if (p_json.GetField(p_key).type == JSONObject.Type.NUMBER) return p_json.GetField(p_key).n.ToString();
            if (p_json.GetField(p_key).type == JSONObject.Type.STRING) return p_json.GetField(p_key).str;
            if (p_json.GetField(p_key).type == JSONObject.Type.BOOL) return p_json.GetField(p_key).b.ToString();

            return "";
        }

        public static bool IsWithinRange(int p_int1, int p_int2, int actualValue)
        {
            return (actualValue >= p_int1 && actualValue <= p_int2);
        }

        // Note that Color32 and Color implictly convert to each other. You may pass a Color object to this method without first casting it.
        string ColorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }

        public static Color HexToColor(string hex)
        {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, 255);
        }

        public static JSONObject JSONListToJSON(List<JSONObject> jsonList)
        {
            JSONObject rawJson = new JSONObject("[]");
            jsonList.ForEach(delegate (JSONObject obj) {
                rawJson.Add(obj);
            });
            return rawJson;
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)System.Enum.Parse(typeof(T), value, true);
        }

        public static T FindValueFromCSV<T>(CSVFile csvFile, string id, string id_column, string target_column)
        {
            for (int i = 0; i < csvFile.length; i++)
            {
                if (csvFile.Get<string>(i, id_column) == id)
                {
                    return csvFile.Get<T>(i, target_column);
                }
            }

            return default(T);
        }

    }
}

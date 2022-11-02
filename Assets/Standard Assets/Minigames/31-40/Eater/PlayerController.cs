using System;
using System.Collections.Generic;
using Components.UnityComponents;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Eater
{
  public class PlayerController : BasicControls
  {
    private const int max_levels = 7;

    [Range(1, max_levels)]
    public int StartFromLevel = 1;
    public AudioClip[] ParseSounds;
    public SpriteRenderer MatrixRender;
    public float MoveSpeed = 15f;
    public float Resist = 5f;
    public Vector2 Bob;

    public string[] Commands_Level_1;
    public string[] Commands_Level_2;
    public string[] Commands_Level_3;
    public string[] Commands_Level_4;
    public string[] Commands_Level_5;
    public string[] Commands_Level_6;
    public string[] Commands_Level_7;

    public GameObject StorageHolder;
    public GameObject CalldataHolder;

    public Text text_version;
    public Text text_main;
    public Text text_parse;
    public Text text_parse_label;
    public Text text_stack;
    public Text text_memory;
    public Text text_desc;
    public Text text_storage;
    public Text text_calldata;
    public Text text_storage_label;
    public Text text_memory_label;

    private Vector2 acceleration_dd;
    private Vector2 velocity_d;
    private Vector2 player_pos;
    private Vector2 bob;

    private MinigameManager gameManager;
    private AudioSource audioSource;

    private Stack<int> stack = new Stack<int>();
    private Stack<int> memory = new Stack<int>();
    private Stack<int> storage = new Stack<int>();
    private Stack<int> calldata = new Stack<int>();

    private int command_index = 0;
    private int current_level = 1;

    private delegate void Parse_delegate(string[] words, bool operation);

    private Parse_delegate[] parse_functions = new Parse_delegate[max_levels + 1];

    private void Start()
    {
      string start_level_key = "StartLevel";
      if (!PlayerPrefs.HasKey(start_level_key))
      {
        PlayerPrefs.SetInt(start_level_key, 0);
      }

      var value = PlayerPrefs.GetInt(start_level_key);
      if (value == 0)
        current_level = this.StartFromLevel;
      else
        current_level = value;

      gameManager = GetComponentInParent<MinigameManager>();
      audioSource = GetComponent<AudioSource>();

      gameManager.ButtonEvents.OnHorizontalPressed += HandleHorizontalStateChange;
      gameManager.ButtonEvents.OnVerticalPressed += HandleVerticalStateChange;
      gameManager.ButtonEvents.OnActionButtonPressed += HandleActionButton;
      player_pos = transform.position;
      this.text_calldata.text = "";
      this.text_parse.text = "";
      this.text_memory.text = "";
      this.text_stack.text = "";
      this.text_storage.text = "";
      this.text_main.text = get_next_command();
      this.text_parse_label.text = "";

      update_stack_output();

      parse_functions[1] = level_1_parse;
      parse_functions[2] = level_2_parse;
      parse_functions[3] = level_3_4_parse;
      parse_functions[4] = level_3_4_parse;
      parse_functions[5] = level_5_parse;
      parse_functions[6] = level_6_7_parse;
      parse_functions[7] = level_6_7_parse;
    }

    private string get_next_command()
    {
      this.stack.Clear();
      this.text_desc.text = "";
      this.text_main.text = "";
      this.text_parse.text = "";
      this.update_stack_output();

      var commands = get_current_commands();
      if (command_index >= commands.Length)
      {
        command_index = 0;
        current_level++;

        if (current_level > max_levels)
        {
          current_level = 1;
        }
        commands = get_current_commands();
      }

      text_version.text = "VERSION " + current_level;

      if (current_level > 3)
      {
        this.calldata.Clear();
        this.calldata.Push(UnityEngine.Random.Range(5, 100));
      }

      if (current_level > 3)
      {
        StorageHolder.SetActive(true);
        CalldataHolder.SetActive(true);
      }
      else
      {
        StorageHolder.SetActive(false);
        CalldataHolder.SetActive(false);
      }

      if (current_level > 6)
      {
        this.memory.Clear();
        this.storage.Clear();
        this.calldata.Clear();
        this.calldata.Push(UnityEngine.Random.Range(1, 5));

        this.text_storage_label.text = "STORAGE (Jim's Balance)";
        this.text_memory_label.text = "MEMORY (Bob's Balance)";
      }
      else
      {
        this.text_storage_label.text = "STORAGE";
        this.text_memory_label.text = "MEMORY";
      }

      return commands[command_index++];
    }

    private string[] get_current_commands()
    {
      switch (current_level)
      {
        case 1: return Commands_Level_1;
        case 2: return Commands_Level_2;
        case 3: return Commands_Level_3;
        case 4: return Commands_Level_4;
        case 5: return Commands_Level_5;
        case 6: return Commands_Level_6;
        case 7: return Commands_Level_7;
      }

      throw new System.Exception($"{current_level} Level is not implemented");
    }

    private void OnDisable()
    {
      gameManager.ButtonEvents.OnHorizontalPressed -= HandleHorizontalStateChange;
      gameManager.ButtonEvents.OnVerticalPressed -= HandleVerticalStateChange;
      gameManager.ButtonEvents.OnActionButtonPressed -= HandleActionButton;
    }

    private void HandleActionButton()
    {
      if (text_main.text.Length == 0 && this.text_parse.text.Length == 0)
      {
        this.text_main.text = get_next_command();
        return;
      }

      if (current_level <= 6)
        this.memory.Clear();

      if (this.text_parse.text.Length == 0)
      {
        this.MatrixRender.enabled = true;
        parse(text_main.text, false);
      }
      else
      {
        this.MatrixRender.enabled = false;
        parse(text_parse.text, true);
      }

      audioSource.PlayOneShot(this.ParseSounds[UnityEngine.Random.Range(0, this.ParseSounds.Length - 1)]);
    }

    private void update_stack_output()
    {
      var str = "";

      var array = stack.ToArray();
      for (int i = 0, c = array.Length; i < array.Length; i++, c--)
      {
        if (current_level >= 5)
          str += $"{c}: {array[i].ToString("X")} [{array[i]}]\n";
        else
          str += $"{c}: {array[i]}\n";
      }
      this.text_stack.text = str;

      str = "";
      array = memory.ToArray();
      for (int i = 0, c = array.Length; i < array.Length; i++, c--)
      {
        if (current_level >= 5)
          str += $"{c}: {array[i].ToString("X")} [{array[i]}]\n";
        else
          str += $"{c}: {array[i]}\n";
      }
      this.text_memory.text = str;

      str = "";
      array = storage.ToArray();
      for (int i = 0, c = array.Length; i < array.Length; i++, c--)
      {
        if (current_level >= 5)
          str += $"{c}: {array[i].ToString("X")} [{array[i]}]\n";
        else
          str += $"{c}: {array[i]}\n";
      }
      this.text_storage.text = str;

      str = "";
      array = calldata.ToArray();
      for (int i = 0, c = array.Length; i < array.Length; i++, c--)
      {
        if (current_level >= 5)
          str += $"{c}: {array[i].ToString("X")} [{array[i]}]\n";
        else
          str += $"{c}: {array[i]}\n";
      }
      this.text_calldata.text = str;
    }

    private void parse(string buffer, bool operate)
    {
      string[] words = buffer.Split(' ');

      if (words.Length == 0)
        return;

      var func = parse_functions[current_level];
      func(words, operate);

      update_stack_output();

      if (operate)
      {
        text_parse.text = "";
        this.text_parse_label.text = "";
        this.text_desc.text = "";
      }

      string label_text = text_parse_label.text;
      if (label_text.Length > 0)
      {
        if (label_text.ToLower().Contains("push"))
          this.text_desc.text = "add value to stack";
        else if (label_text.ToLower().Contains("pop"))
          this.text_desc.text = "load value from stack";
        else if (label_text.ToLower().Contains("add"))
          this.text_desc.text = "add two last values from stack and pushes back result";
        else if (label_text.ToLower().Contains("sub"))
          this.text_desc.text = "subtracts two last values from stack and pushes back result";
        else if (label_text.ToLower().Contains("mul"))
          this.text_desc.text = "multiples two last values from stack and pushes back result";
        else if (label_text.ToLower().Contains("dup"))
          this.text_desc.text = "reads and pushes last value from stack";
        else if (label_text.ToLower().Contains("calldata"))
          this.text_desc.text = "read parameters and push to stack";
        else if (label_text.ToLower().Contains("sstore"))
          this.text_desc.text = "pops value from stack and stores in storage";
        else if (label_text.ToLower().Contains("mstore"))
          this.text_desc.text = "pops value from stack and stores in memory";
        else if (label_text.ToLower().Contains("sload"))
          this.text_desc.text = "reads value from storage and pushes it to stack";
        else if (label_text.ToLower().Contains("mload"))
          this.text_desc.text = "reads value from memory and pushes it to stack";
      }
    }

    private string read_chars(string buffer, int pos, int char_count = 2)
    {
      return buffer.Substring(pos, char_count);
    }

    private void level_6_7_parse(string[] words, bool operate)
    {
      int pos = 0;
      string buffer = words[0];
      if (buffer.StartsWith("0x"))
        buffer = buffer.Substring(2); // skip 0x

      var current_cmd = read_chars(buffer, pos);
      pos += 2;

      switch (current_cmd)
      {
        case "60":
        case "61":
        case "62":
        case "63":
        case "64":
        case "6e":
        case "70":
        case "73":
          {
            var converted = Convert.ToInt32(current_cmd, 16);
            var chars_to_read = converted - 96 + 1;
            chars_to_read *= 2;
            var args = read_chars(buffer, pos, chars_to_read);
            var converted_args = Convert.ToInt32(args, 16);
            pos += chars_to_read;

            if (operate)
              stack.Push(converted_args);

            this.text_parse_label.text = $"PUSH{chars_to_read / 2} {args}";
          }
          break;

        case "50": // pop
          {
            if (operate)
              stack.Pop();

            this.text_parse_label.text = $"POP";
          }
          break;

        case "80": // dup
          {
            if (operate)
            {
              var stack_val_0 = stack.Pop();

              stack.Push(stack_val_0);
              stack.Push(stack_val_0);
            }
            this.text_parse_label.text = $"DUP";
          }
          break;

        case "01":
          { // add
            if (operate)
            {
              var stack_val_0 = stack.Pop();
              var stack_val_1 = stack.Pop();

              stack.Push(stack_val_0 + stack_val_1);

            }
            this.text_parse_label.text = $"ADD";
          }
          break;

        case "02":
          { // mul
            if (operate)
            {
              var stack_val_0 = stack.Pop();
              var stack_val_1 = stack.Pop();

              stack.Push(stack_val_0 * stack_val_1);
            }
            this.text_parse_label.text = $"MUL";
          }
          break;

        case "03":
          { // subtract
            if (operate)
            {
              var stack_val_0 = stack.Pop();
              var stack_val_1 = stack.Pop();

              stack.Push(stack_val_0 - stack_val_1);

            }
            this.text_parse_label.text = $"SUB";
          }
          break;

        case "35": // calldataload
          {
            if (operate)
            {
              var calldata_val_0 = calldata.Peek();

              stack.Push(calldata_val_0);
            }
            this.text_parse_label.text = $"CALLDATA";
          }
          break;

        case "51": // mload
          {
            if (operate)
            {
              var memory_value = memory.Peek();

              stack.Push(memory_value);
            }
            this.text_parse_label.text = $"MLOAD";
          }
          break;

        case "52": // mstore
          {
            if (operate)
            {
              var stack_val_0 = stack.Pop();

              memory.Clear();
              memory.Push(stack_val_0);
            }
            this.text_parse_label.text = $"MSTORE";
          }
          break;

        case "54": // sload
          {
            if (operate)
            {
              var storage_value = storage.Peek();

              stack.Push(storage_value);
            }
            this.text_parse_label.text = $"SLOAD";
          }
          break;
        case "55": // sstore
          {
            if (operate)
            {
              var stack_val_0 = stack.Pop();

              storage.Clear();
              storage.Push(stack_val_0);
            }
            this.text_parse_label.text = $"SSTORE";
          }
          break;

        default: throw new System.Exception($"{current_cmd} was not found");
      }

      if (operate)
        return;

      text_main.text = buffer.Substring(pos);

      text_parse.text = buffer.Substring(0, pos);
    }

    private void level_5_parse(string[] words, bool operate)
    {
      int pos = 0;
      string buffer = words[0];
      if (buffer.StartsWith("0x"))
        buffer = buffer.Substring(2); // skip 0x
      var current_cmd = read_chars(buffer, pos);
      pos += 2;

      switch (current_cmd)
      {
        // push
        case "0a":
        case "0b":
        case "0c":
        case "0d":
        case "0e":
        case "0f":
        case "10":
        case "11":
        case "12":
        case "13":
          {
            var converted = Convert.ToInt32(current_cmd, 16);
            var chars_to_read = converted - 10 + 1;
            var args = read_chars(buffer, pos, chars_to_read);
            var converted_args = Convert.ToInt32(args, 16);
            pos += chars_to_read;

            if (operate)
              stack.Push(converted_args);

            this.text_parse_label.text = $"PUSH{chars_to_read} {args}";
          }
          break;

        case "03": // pop
          {
            if (operate)
            {
              var stack_val_0 = stack.Pop();

              memory.Push(stack_val_0);
            }
            this.text_parse_label.text = $"POP";
          }
          break;

        case "04": // dup
          {
            if (operate)
            {
              var stack_val_0 = stack.Pop();

              stack.Push(stack_val_0);
              stack.Push(stack_val_0);
            }
            this.text_parse_label.text = $"DUP";
          }
          break;

        case "02":
          { // add
            if (operate)
            {
              var stack_val_0 = stack.Pop();
              var stack_val_1 = stack.Pop();

              stack.Push(stack_val_0 + stack_val_1);

            }
            this.text_parse_label.text = $"ADD";
          }
          break;

        case "05":
          { // mul
            if (operate)
            {
              var stack_val_0 = stack.Pop();
              var stack_val_1 = stack.Pop();

              stack.Push(stack_val_0 * stack_val_1);
            }
            this.text_parse_label.text = $"MUL";
          }
          break;

        case "06": // calldata
          {
            if (operate)
            {
              var calldata_val_0 = calldata.Peek();

              stack.Push(calldata_val_0);
            }
            this.text_parse_label.text = $"CALLDATA";
          }
          break;

        case "07": // sstore
          {
            if (operate)
            {
              var stack_val_0 = stack.Pop();

              storage.Push(stack_val_0);
            }
            this.text_parse_label.text = $"SSTORE";
          }
          break;

        default:
          {
            throw new System.Exception($"{current_cmd} was not found");
          }
      }

      if (operate)
        return;

      text_main.text = buffer.Substring(pos);

      text_parse.text = buffer.Substring(0, pos);
    }

    private void level_3_4_parse(string[] words, bool operate)
    {
      int pos = 0;
      string buffer = words[0];
      var current_cmd = read_chars(buffer, pos);
      pos += 2;

      switch (current_cmd)
      {
        case "11":
        case "12":
        case "13":
        case "14":
        case "15":
        case "16":
        case "17":
        case "18":
        case "19": // push
          {
            var chars_to_read = int.Parse(current_cmd) - 10;
            var args = read_chars(buffer, pos, chars_to_read);
            pos += chars_to_read;

            if (operate)
              stack.Push(int.Parse(args));

            this.text_parse_label.text = $"PUSH{chars_to_read} {args}";
          }
          break;

        case "03": // pop
          {
            if (operate)
            {
              var stack_val_0 = stack.Pop();

              memory.Push(stack_val_0);
            }
            this.text_parse_label.text = $"POP";
          }
          break;

        case "02":
          { // add
            if (operate)
            {
              var stack_val_0 = stack.Pop();
              var stack_val_1 = stack.Pop();

              stack.Push(stack_val_0 + stack_val_1);

            }
            this.text_parse_label.text = $"ADD";
          }
          break;

        case "04": // dup
          {
            if (operate)
            {
              var stack_val_0 = stack.Pop();

              stack.Push(stack_val_0);
              stack.Push(stack_val_0);
            }
            this.text_parse_label.text = $"DUP";
          }
          break;

        case "05":
          { // mul
            if (operate)
            {
              var stack_val_0 = stack.Pop();
              var stack_val_1 = stack.Pop();

              stack.Push(stack_val_0 * stack_val_1);
            }
            this.text_parse_label.text = $"MUL";
          }
          break;

        case "06": // calldata
          {
            if (operate)
            {
              var calldata_val_0 = calldata.Peek();

              stack.Push(calldata_val_0);
            }
            this.text_parse_label.text = $"CALLDATA";
          }
          break;

        case "07": // sstore
          {
            if (operate)
            {
              var stack_val_0 = stack.Pop();

              storage.Push(stack_val_0);
            }
            this.text_parse_label.text = $"SSTORE";
          }
          break;

        default:
          {
            throw new System.Exception($"{current_cmd} was not found");
          }
      }

      if (operate)
        return;

      text_main.text = buffer.Substring(pos);

      text_parse.text = buffer.Substring(0, pos);
    }

    private void level_2_parse(string[] words, bool operate)
    {
      int pos = 0;
      string current_cmd = words[pos].ToLower();

      switch (current_cmd)
      {
        case "1": // push
          {
            var value = int.Parse(words[++pos]);

            if (operate)
              stack.Push(value);

            this.text_parse_label.text = $"PUSH {value}";
          }
          break;

        case "2": // add
          {
            if (operate)
            {
              var stack_val_0 = stack.Pop();
              var stack_val_1 = stack.Pop();

              stack.Push(stack_val_0 + stack_val_1);
            }
            this.text_parse_label.text = $"ADD";
          }
          break;

        case "3": // pop
          {
            if (operate)
            {
              var stack_val_0 = stack.Pop();

              memory.Push(stack_val_0);
            }
            this.text_parse_label.text = $"POP";
          }
          break;

        case "4":// dup
          {
            if (operate)
            {
              var stack_val_0 = stack.Pop();

              stack.Push(stack_val_0);
              stack.Push(stack_val_0);
            }
            this.text_parse_label.text = $"DUP";
          }
          break;

        case "5": // mul
          {
            if (operate)
            {
              var stack_val_0 = stack.Pop();
              var stack_val_1 = stack.Pop();

              stack.Push(stack_val_0 * stack_val_1);
            }
            this.text_parse_label.text = $"MUL";
          }
          break;

        default:
          {
            throw new System.Exception($"{current_cmd} was not found");
          }
      }

      pos++;

      if (operate)
        return;

      var result = "";
      for (var i = pos; i < words.Length; i++)
      {
        result += words[i] + " ";
      }
      text_main.text = result.Trim();

      var last_operation = "";
      for (int i = 0; i < pos; i++)
      {
        last_operation += words[i] + " ";
      }
      text_parse.text = last_operation.Trim();
    }

    private void level_1_parse(string[] words, bool operate)
    {
      int pos = 0;
      string current_cmd = words[pos].ToLower();

      switch (current_cmd)
      {
        case "push":
          {
            var value = int.Parse(words[++pos]);

            if (operate)
              stack.Push(value);
          }
          break;

        case "pop":
          {
            if (operate)
            {
              var stack_val_0 = stack.Pop();

              memory.Push(stack_val_0);
            }
          }
          break;

        case "dup":
          {
            if (operate)
            {
              var stack_val_0 = stack.Pop();

              stack.Push(stack_val_0);
              stack.Push(stack_val_0);
            }
          }
          break;

        case "add":
          {
            if (operate)
            {
              var stack_val_0 = stack.Pop();
              var stack_val_1 = stack.Pop();

              stack.Push(stack_val_0 + stack_val_1);
            }
          }
          break;
        case "mul":
          {
            if (operate)
            {
              var stack_val_0 = stack.Pop();
              var stack_val_1 = stack.Pop();

              stack.Push(stack_val_0 * stack_val_1);
            }
          }
          break;

        default:
          {
            throw new System.Exception($"{current_cmd} was not found");
          }
      }

      pos++;

      if (operate)
        return;

      var result = "";
      for (var i = pos; i < words.Length; i++)
      {
        result += words[i] + " ";
      }
      text_main.text = result.Trim();

      var last_operation = "";
      for (int i = 0; i < pos; i++)
      {
        last_operation += words[i] + " ";
      }
      text_parse.text = last_operation.Trim();
    }

    private void Update()
    {
      acceleration_dd.x = (float)HorizontalState;
      acceleration_dd.y = (float)VerticalState;

      var acceleration_squared = Vector2.Dot(acceleration_dd, acceleration_dd);

      if (acceleration_squared > 1.0f)
      {
        acceleration_dd *= 1.0f / Mathf.Sqrt(acceleration_squared);
      }

      acceleration_dd *= MoveSpeed;
      acceleration_dd += -Resist * velocity_d;
      // p_d = (1/2) * at^2 + vt + v
      float dt = Time.deltaTime;
      Vector2 player_delta = 0.5f * acceleration_dd * Mathf.Pow(dt, 2) + velocity_d * dt;

      // v' = at + v
      velocity_d += acceleration_dd * dt;

      // p' = p + p_d
      player_pos += player_delta;

      bob += new Vector2(dt, dt);

      if (bob.x > 2.0f * Mathf.PI)
        bob.x -= 2.0f * Mathf.PI;

      if (bob.y > 2.0f * Mathf.PI)
        bob.y -= 2.0f * Mathf.PI;

      player_pos += new Vector2(
        this.Bob.x * Mathf.Cos(bob.x),
        this.Bob.y * Mathf.Sin(bob.y)
      );

      transform.position = player_pos;
    }
  }
}

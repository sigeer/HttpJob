﻿using SpiderTool.Data.Dto.Spider;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using Utility.SimpleStringParse;

namespace SpiderWin.Modals
{
    public partial class ReplaceTextForm : Form
    {
        List<ReplacementRuleDto> ReplacementRules = new List<ReplacementRuleDto>();
        public ReplaceTextForm()
        {
            InitializeComponent();

            TextFilePath.AllowDrop = true;
            ReadConfig(1);
        }

        private void SetWorking(bool isWorking)
        {
            BtnDo.Enabled = !isWorking;
        }
        private void BtnDo_Click(object sender, EventArgs e)
        {
            var txtFilePath = TextFilePath.Text;
            if (string.IsNullOrEmpty(txtFilePath) || !File.Exists(txtFilePath))
            {
                MessageBox.Show($"文件{txtFilePath}不存在");
                return;
            }

            if (ReplacementRules.Count == 0)
            {
                MessageBox.Show("无规则，无需替换");
                return;
            }

            Task.Run(() =>
            {
                Invoke(() =>
                {
                    SetWorking(true);
                });

                var workRules = FormatReplaceRulesDynamic(ReplacementRules);
                var content = File.ReadAllText(txtFilePath);
                foreach (var rule in workRules)
                {
                    if (rule.UseRegex)
                        content = Regex.Replace(content, rule.ReplacementOldStr, rule.ReplacementNewlyStr ?? string.Empty);
                    else
                        content = content.Replace(rule.ReplacementOldStr, rule.ReplacementNewlyStr ?? string.Empty);
                }
                File.WriteAllText(txtFilePath, content);
                MessageBox.Show("操作完成");
                Invoke(() =>
                {
                    SetWorking(false);
                });
            });
        }

        protected virtual List<ReplacementRuleDto> FormatReplaceRulesDynamic(List<ReplacementRuleDto> rules)
        {
            var newList = new List<ReplacementRuleDto>();
            var provider = new SimpleStringTokenProvider();
            foreach (var rule in rules)
            {
                var oldValue = provider.Serialize(rule.ReplacementOldStr);
                var newlyValue = provider.Serialize(rule.ReplacementNewlyStr);
                newList.Add(new ReplacementRuleDto
                {
                    Id = rule.Id,
                    UseRegex = rule.UseRegex,
                    ReplacementOldStr = oldValue,
                    ReplacementNewlyStr = newlyValue
                });
            }
            return newList;
        }

        private void Btn_OpenDir_Click(object sender, EventArgs e)
        {
            var selectedFile = TextFilePath.Text;
            if (!string.IsNullOrEmpty(selectedFile) && File.Exists(selectedFile))
            {
                Process.Start("explorer.exe", Path.GetDirectoryName(selectedFile)!);
            }
        }

        private void SetOpenDirVisible()
        {
            Btn_OpenDir.Visible = !string.IsNullOrEmpty(TextFilePath.Text);
        }

        private void BtnSelectFile_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                TextFilePath.Text = dialog.FileName;
            }
        }

        private void TextFilePath_DragDrop(object sender, DragEventArgs e)
        {
            var dragData = e.Data?.GetData(DataFormats.FileDrop);
            // 获取拖放的文件路径数组

            string[]? files = dragData == null ? null : (string[])dragData;

            if (files?.Length > 0)
                TextFilePath.Text = files[0];
        }

        private void TextFilePath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) != null)
            {
                // 允许拖放操作
                e.Effect = DragDropEffects.Copy;
            }
        }


        private string ConfigFile(int index)
        {
            var configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"x_config{index}.cache");
            return configFile;
        }

        private void ShowConfig(int index)
        {
            var dialog = new TxtReplaceRuleManageForm(ReplacementRules);
            dialog.OnOk += (obj, evt) =>
            {
                ReplacementRules = evt;
                File.WriteAllText(ConfigFile(index), JsonSerializer.Serialize(ReplacementRules));
            };
            dialog.ShowDialog();
        }

        private void ReadConfig(int index)
        {
            _currentSelectedConfig = index;
            var configBtns = new Button[] { BtnConfig1, BtnConfig2, BtnConfig3 };
            for (int i = 0; i < configBtns.Length; i++)
            {
                if (i == index - 1)
                {
                    configBtns[i].Text = $"设置{i + 1} ✔";
                    configBtns[i].ForeColor = Color.Green;
                }
                else
                {
                    configBtns[i].Text = $"设置{i + 1}";
                    configBtns[i].ForeColor = Color.Black;
                }

            }
            var cfg = ConfigFile(index);
            if (File.Exists(cfg))
                ReplacementRules = JsonSerializer.Deserialize<List<ReplacementRuleDto>>(File.ReadAllText(cfg) ?? "[]") ?? new List<ReplacementRuleDto>();
            else
                ReplacementRules = new List<ReplacementRuleDto>();
        }
        int _currentSelectedConfig = 1;
        private void BtnConfig1_Click(object sender, EventArgs e)
        {
            if (_currentSelectedConfig == 1)
                ShowConfig(1);
            else
                ReadConfig(1);
        }

        private void BtnConfig2_Click(object sender, EventArgs e)
        {
            if (_currentSelectedConfig == 2)
                ShowConfig(2);
            else
                ReadConfig(2);
        }

        private void BtnConfig3_Click(object sender, EventArgs e)
        {
            if (_currentSelectedConfig == 3)
                ShowConfig(3);
            else
                ReadConfig(3);
        }

        private void TextFilePath_TextChanged(object sender, EventArgs e)
        {
            SetOpenDirVisible();
        }
    }
}

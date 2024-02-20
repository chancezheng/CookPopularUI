﻿/*
 * Description：DialogWindow 
 * Author： Chance.Zheng
 * Create Time: 2023-03-08 12:43:57
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using CookPopularToolkit.Windows;
using CookPopularUI.WPF.Controls;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ValueBoxes = CookPopularToolkit.ValueBoxes;

namespace CookPopularUI.WPF.Windows
{
    /// <summary>
    /// 表示对话框窗体
    /// </summary>
    public class DialogWindow : NormalWindow, IDialog
    {
        public bool IsClosed { get; private set; }
        /// <summary>
        /// 是否点击了确定，如果true，则将对话框的内容传至发起界面，false则不响应
        /// </summary>
        public bool IsConfirm { get; private set; }

        /// <summary>
        /// 是否显示窗体Icon图标
        /// </summary>
        public bool IsShowIcon
        {
            get => (bool)GetValue(IsShowIconProperty);
            set => SetValue(IsShowIconProperty, ValueBoxes.BooleanBox(value));
        }
        /// <summary>
        /// 提供<see cref="IsShowIcon"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsShowIconProperty =
            DependencyProperty.Register(nameof(IsShowIcon), typeof(bool), typeof(DialogWindow), new PropertyMetadata(ValueBoxes.TrueBox));


        static DialogWindow()
        {
            StyleProperty.OverrideMetadata(typeof(DialogWindow), new FrameworkPropertyMetadata(ResourceHelper.GetResource<Style>("DialogWindowStyle")));
        }

        private DialogWindow()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.ConfirmCommand, Executed, (s, e) => e.CanExecute = true));
            CommandBindings.Add(new CommandBinding(ControlCommands.CancelCommand, Executed, (s, e) => e.CanExecute = true));

            this.Owner = WindowHelper.GetActiveWindow();
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        private static void Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var win = sender as DialogWindow;
            if (win.IsNull()) return;

            if (e.Command == ControlCommands.ConfirmCommand)
                win!.IsConfirm = true;
            else if (e.Command == ControlCommands.CancelCommand)
                win!.IsConfirm = false;

            win!.Close();
        }

        public void CloseWindow()
        {
            this.Close();
            IsClosed = true;
        }

        public static DialogWindow Show<T>(string? caption = default, bool isShowIcon = true) where T : new()
        {
            return Show(new T(), caption, isShowIcon);
        }

        public static DialogWindow Show(object content, string? caption = default, bool isShowIcon = true)
        {
            var win = new DialogWindow();
            win.Title = caption ?? string.Empty;
            win.Content = content;
            win.IsShowIcon = isShowIcon;
            win.Show();

            return win;
        }

        public static DialogWindow ShowDialog<T>(string? caption = default, bool isShowIcon = true) where T : new()
        {
            return ShowDialog(new T(), caption, isShowIcon);
        }

        public static DialogWindow ShowDialog(object content, string? caption = default, bool isShowIcon = true)
        {
            var win = new DialogWindow();
            win.Title = caption ?? string.Empty;
            win.Content = content;
            win.IsShowIcon = isShowIcon;
            win.ShowDialog();

            return win;
        }
    }
}

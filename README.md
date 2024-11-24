# Windows Form 加入載入動畫 Adding a Loading Animation to Windows Forms
---
在 Windows Forms 應用程式中加入「載入動畫」並不簡單：解決「UI 執行緒阻塞」問題。
以下是說明、實作步驟及範例程式碼。

Adding a loading animation to a Windows Forms application is not straightforward due to the "UI thread blocking" issue. 
Here is an explanation, implementation steps, and sample code.

## 什麼是「UI 執行緒阻塞」？ What is "UI Thread Blocking"?
當一個表單（Form）取得操作焦點時，其他在背景運行的表單將暫停動作。
因此，當載入動畫出現時，必須另開執行緒在背景中執行接著要顯示的新表單，才能讓動畫持續播放。

When a form gains focus, other forms running in the background will pause their actions. 
Therefore, when the loading animation appears, 
it is necessary to open another thread in the background to execute the new form to be displayed, 
allowing the animation to keep playing.

## 主執行緒的控制權問題 Control Issue of the Main Thread
這時又產生了一個問題...
由於新表單是透過另一個執行緒開啟的，當新表單顯示時，該執行緒會取得控制權成為主執行緒，
而負責動畫的執行緒則失去控制權，使得關閉動畫的操作無法順利執行。
因此，必須將「關閉動畫」的指令交由處理新表單的執行緒來執行。

This raises another issue... 
Since the new form is opened through a different thread, 
that thread gains control and becomes the main thread when the new form is displayed. 
Meanwhile, the thread responsible for the animation loses control, making it difficult to close the animation properly. 
Thus, the "close animation" command must be handled by the thread managing the new form.

## 完整實作流程 Complete Implementation Process
1. 以對話框的形式顯示載入動畫表單，鎖住其他表單並強制使用者等待。

   Display the loading animation form as a dialog, locking other forms and forcing the user to wait.

2. 在背景執行緒中初始化新表單，然後關閉動畫並顯示新表單。

   Initialize the new form in a background thread, then close the animation and display the new form.

3. 等待背景執行緒完成操作。

   Wait for the background thread to complete its operation.

藉由使用 async / await 關鍵字，能用最少的程式碼完成以上流程。

Using async/await keywords enables completing the above process with minimal code.

## 範例程式 Sample Code

### 表單及元件配置： Form and Component Configuration:
1. 建立 3 張 Windows Form：Form1、Form2及FormLoading。

   Create 3 Windows Forms: Form1, Form2, and FormLoading.

2. 在 FormLoading 表單中加入一個 PictureBox 元件，
   放入 loading.gif 作為載入動畫（GIF 來源：GIFER 網站：https://gifer.com/en/ZKZg）。

   Add a PictureBox component to the FormLoading form and set loading.gif as the loading animation (GIF source: GIFER website).
   
3. 在 Form1 放置 1 個 button，name 屬性為「btnOpenForm2」, 顯示文字是「Open Form2」。

   Place a button on Form1 with the name attribute set to "btnOpenForm2" and display text as "Open Form2".

4. Form2 放置 1 個 Lable 元件顯示文字「load complete」，並在 Form2 初始化時等待 5 秒模擬載入延遲。

   Place a Label component on Form2 to display the text "load complete," and simulate a loading delay of 5 seconds during Form2's initialization.

5. 在 btnOpenForm2 的 Click 事件中，加入程式碼以背景執行緒開啟新表單，並控制動畫的關閉與新表單的顯示。

   Add code to the Click event of btnOpenForm2 to open the new form in a background thread, managing the closing of the animation and displaying the new form.

### Form1.cs
```
private async void btnOpenForm2_Click(object sender, EventArgs e)
{
    using (var formLoading = new FormLoading())
    {
        // 在背景執行緒初始化 Form2，完成後關閉動畫再顯示 Form2
		// Initialize Form2 in a background thread, then close the animation and display Form2.
        var loadForm2Task = Task.Run(() =>
        {
            var form2 = new Form2();
            formLoading.DialogResult = DialogResult.OK;
            form2.ShowDialog();
        });

        // 以Dialog顯示loading form，鎖住其他的表單強迫使用者等待
        // Display the loading form as a dialog, locking other forms and forcing the user to wait.
        formLoading.ShowDialog();

        // 等待 Form2 完成初始化並顯示
		// Wait for F2 to complete its operation.
        await loadForm2Task;
    }
}
```

### Form2.cs
```
public Form2()
{
    InitializeComponent();
    LoadData();
}

private void LoadData()
{
    // 模擬載入時間 (非同步等待 5 秒)
	// Simulate a loading delay of 5 seconds
    Thread.Sleep(5000);

    Label lbl = new Label { Text = "loading complete", AutoSize = true };
    this.Controls.Add(lbl);
}
```

### FormLoading.cs
```
public FormLoading()
{
    InitializeComponent();
    this.StartPosition = FormStartPosition.CenterParent;
    this.FormBorderStyle = FormBorderStyle.None;
}
```
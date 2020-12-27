// 源码位置：第二章\EventsDemo.cs
// 编译命令：csc EventsDemo.cs
using System;

public class EventsDemo
{
    static void Main()
    {
        var publisher = new Publisher();
        var subscriber = new Subscriber();

        publisher.ButtonClicked += subscriber.ProcessClick;
        publisher.OnEditing += subscriber.ProcessNewText;

        publisher.RaiseTextInput();
        publisher.RaiseButtonClicked();

        Console.WriteLine("演示移除事件监听程序的方法");
        publisher.OnEditing -= subscriber.ProcessNewText;
        publisher.RaiseTextInput();
        publisher.RaiseButtonClicked();
    }
}

public class TextInputEventArgs : EventArgs
{
    public TextInputEventArgs(string text) { Text = text; }
    public string Text { get; }
}

public class Publisher
{
    public delegate void TextInputEvent(object sender, TextInputEventArgs args);

    // 使用泛型版的EventArgs<T>可以免掉每次定义新委托的麻烦
    // 下面注释的事件定义与更下面一行的事件定义等价
    // public event EventHandler<TextInputEventArgs> OnEditing;
    public event TextInputEvent OnEditing;

    public event EventHandler ButtonClicked;

    public void RaiseTextInput()
    {
        if (OnEditing != null)
            OnEditing(this, new TextInputEventArgs("Some text"));
    }

    public void RaiseButtonClicked()
    {
        if (ButtonClicked != null)
            ButtonClicked(this, null);
    }
}

public class Subscriber
{
    public void ProcessNewText(object sender, TextInputEventArgs args)
    {
        Console.WriteLine("Got new text: {0}", args.Text);
    }

    public void ProcessClick(object sender, EventArgs args)
    {
        Console.WriteLine("Button clicked");
    }
}
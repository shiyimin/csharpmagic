# -*- coding: utf-8 -*-
import clr;
clr.AddReference("System.Windows.Forms")
from System.Windows.Forms import *
f = Form()
f.MaximizeBox = False
f.MinimizeBox = True
f.Text = "演示Winform"

clr.AddReference("System.Drawing")
from System.Drawing import Point
button1 = Button()
button1.Text = "OK"
button1.Location = Point(10, 10)
f.Controls.Add(button1)

def on_button1_ok(sender, args):
    print "按下了OK按钮"

button1.Click += on_button1_ok

f.ShowDialog()

from System.Guid import NewGuid
NewGuid()

from System.Collections.Generic import List, Dictionary
int_list = List[int]()
str_float_dict = Dictionary[str, float]()

int_list.Add(1)
int_list[0]

str_float_dict.Add("key", 123.456)
str_float_dict["key"]

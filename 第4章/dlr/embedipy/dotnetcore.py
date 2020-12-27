import md5
print csvariable
m = md5.new(csvariable.encode('utf-8')).hexdigest()
print m
csvariable = "Value from python"
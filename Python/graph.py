import matplotlib.pyplot as plt

counts = []
data1 = []
data2 = []
data3 = []
data4 = []


def show(figurecount, counts, data1, data2):
    plt.figure(figurecount)
    plt.plot(counts, data1)
    plt.plot(counts, data2)
    plt.xlabel("Datapoints count")
    plt.ylabel("Time in miliseconds")
    plt.grid(True)
    plt.axes([0, max(counts), 0, max(data2)])


filepath = "Logs/log 05-04-2020 19.32.10.csv"
datafile = open(filepath, "r")
for line in datafile:
    data = line.split(';')
    counts.append(int(data[1]))
    data1.append(int(data[2]))
    data2.append(int(data[3]))
    data3.append(int(data[4]))
    data4.append(int(data[5]))

datafile.close()

show(1, counts, data1, data2)
show(2, counts, data3, data4)
plt.show()

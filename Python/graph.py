import matplotlib.pyplot as plt
import glob

logsdir = "Logs/"
graphsdir = "Python/graphs/"

counts = []
data1 = []
data2 = []
data3 = []
data4 = []


def analyze(filepath):
    datafile = open(filepath, "r")
    for line in datafile:
        data = line.split(';')
        counts.append(int(data[1]))
        data1.append(int(data[2]) / 1000)
        data2.append(int(data[3]) / 1000)
        data3.append(int(data[4]) / 1000)
        data4.append(int(data[5]) / 1000)

    datafile.close()

    datafilename = filepath.split('/').pop()
    plot(datafilename, 1, counts, data1, data2)
    plot(datafilename, 2, counts, data3, data4)


def plot(datafile, figurecount, counts, data1, data2):
    path = f"{graphsdir}{datafile} {figurecount}.png"
    plt.figure(figurecount)
    plt.plot(counts, data1)
    plt.plot(counts, data2)
    plt.xlabel("Datapoints count")
    plt.ylabel("Time in seconds")
    plt.grid(True)
    plt.axes([0, max(counts), 0, max(data2)])
    plt.savefig(path)


logfiles = glob.glob(f"{logsdir}/*.csv")
for file in logfiles:
    analyze(file)

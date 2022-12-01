from Pyro4 import expose
import random

#from PIL import Image
#import numpy as np

class Solver:
    def __init__(self, workers=None, input_file_name=None, output_file_name=None):
        self.input_file_name = input_file_name
        self.output_file_name = output_file_name
        self.workers = workers
        print("Inited")

    def solve(self):
        print("Job Started")
        print("Workers %d" % len(self.workers))

        K = 30 # the number of colors in the image
        max_iters = 20
        X = []

        X = self.read_input()

        # map
        mapped = []
        for i in xrange(0, len(self.workers)):
            print("map %d" % i)
            mapped.append(self.workers[i].mymap(X[(i*len(X)//len(self.workers)) : ((i+1)*len(X)//len(self.workers))], K//len(self.workers), max_iters))

        # reduce
        colors = self.myreduce(mapped)

        # Indexes for color for each pixel
        idx = Solver.find_closest_centroids(X, colors)

        # Reconstruct the image
        res = []
        for i in range(len(idx)):
            for k in range(K):
                if (idx[i] == k):
                        res.append(colors[k])
                        break

        # output
        self.write_output(res)

        print("Job Finished")

    @staticmethod
    @expose
    def mymap(X, K, max_iters):
        centroids = Solver.initialize_K_centroids(X, K)
        previous_centroids = centroids
        for _ in range(max_iters):
            idx = Solver.find_closest_centroids(X, centroids)
            centroids = Solver.compute_means(X, idx, K)
            for i in range(len(centroids)):
                for j in range(len(centroids[i])):
                    if (previous_centroids[i][j]==centroids[i][j]):
                        # The centroids aren't moving anymore.
                        return centroids

            previous_centroids = centroids

        return centroids

    @staticmethod
    @expose
    def myreduce(mapped):
        print("reduce")
        output = []

        for color in mapped:
            print("reduce loop")
            output = output + color.value
        print("reduce done")
        return output

    def read_input(self):
        X = []
        with open(self.input_file_name, "r") as f:
            for line in f:
                items = line.split()
                X.append([float(items[0][1:-1])] + [float(item[:-1]) for item in items[1:]])
        return(X)

    def write_output(self, X):
        with open(self.output_file_name, 'w') as f:
            for x in X:
                f.write("%s\n" % x)
        print("output done")

    @staticmethod
    @expose
    def initialize_K_centroids(X, K):
        C = []
        for i in range(K):
            C.append(random.choice(X))
        return C

    @staticmethod
    @expose
    def find_closest_centroids(X, centroids):
        m = len(X)
        c = [0]*len(X)

        for i in range(m):
            # Find distances
            distances = []
            distance = 0
            for j in range(len(centroids)):
                for k in range(len(centroids[j])):
                    distance += (X[i][k] - centroids[j][k])**2
                distances.append(distance)
                distance = 0

            # Assign closest cluster to c[i]
            c[i] = distances.index(min(distances))
    
        return c

    @staticmethod
    @expose
    def compute_means(X, idx, K):
        centroids = []
        examples = []
        mean = [0] * len(X[0])

        for k in range(K):
            examples = []
            for i in range(len(X[0])):
                mean[i] = 0

            for i in range(len(X)):
                if (idx[i] == k):
                    examples.append(X[i])

            for i in range(len(examples)):
                for j in range(len(X[0])):
                    mean[j] += examples[i][j]

            for i in range(len(X[0])):
                mean[i] /= len(examples)

            centroids.append(mean[:])

        return centroids

"""
def load_image(path):
    image = Image.open(path)
    return np.asarray(image) / 255

def main():

    image_path = "300x300.png"
    K = 30
    max_iters = 20

    # Load the image
    image = load_image(image_path)
    w, h, d = image.shape
    print('Image found with width: {}, height: {}, depth: {}'.format(w, h, d))

    # Get the feature matrix X
    X = image.reshape((w * h, d))

    X.tolist()

    with open("Picture.txt", 'w') as f:
        for x in X:
            f.write("%s\n" % x.tolist())
    f.close()


    ###################################################################################

    X = []
    with open("output.txt", "r") as f:
        for line in f:
            items = line.split()
            X.append([float(items[0][1:-1])] + [float(item[:-1]) for item in items[1:]])

    idx = np.array(X) * 255
    idx = np.array(idx, dtype=np.uint8)
    X_reconstructed = idx.reshape((100, 100, 3))
    compressed_image = Image.fromarray(X_reconstructed)

    # Save reconstructed image to disk
    compressed_image.save('out.png')


if __name__ == '__main__':
    main()
"""

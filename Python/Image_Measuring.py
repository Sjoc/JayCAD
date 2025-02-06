import cv2
import numpy

class ImageMeasure:

    def __init__(self):
        self.m_image = None
        self.point1 = None
        self.point2 = None
        self.scaleFactor = 0.27032019704433497536945812807882

    def SetScaleFactor(self, factor):
        self.scaleFactor = factor

    def SetImage(self, image):
        self.m_image = image

    def Measure(self):
        self.point1
        self.point2
        if self.point1 != None and self.point2 != None:
            xDifference = self.scaleFactor * abs(self.point1[0] - self.point2[0])
            yDifference = self.scaleFactor * abs(self.point1[1] - self.point2[1])
            print("X Distance: " + str(xDifference))
            print("Y Distance: " + str(yDifference))
            self.point1 = None
            self.point2 = None

    def PointPicker(self, event, x, y, flags, param):
        self.point1
        self.point2
        if event == cv2.EVENT_LBUTTONDOWN:
                CP = [x, y]
                print("Position: "+ str(CP))
                if point1 == None:
                    point1 = CP
                else:
                    point2 = CP
                Measure()



    image = cv2.imread(imageString)
    cv2.namedWindow(imageName)
    cv2.setMouseCallback(imageName, PointPicker);
    cv2.imshow(imageName, image)

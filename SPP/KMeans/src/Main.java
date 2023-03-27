import java.io.*;
import parcs.*;
import java.awt.Graphics2D;
import java.awt.image.BufferedImage;
import javax.imageio.ImageIO;
import java.util.Arrays;

public class Main implements AM 
{
    public static void main(String[] args) {
        task curtask = new task();
        curtask.addJarFile("KMeans.jar");
        (new Main()).run(new AMInfo(curtask, (channel)null));
        curtask.end();
    }

    public void run(AMInfo info) {
		BufferedImage originalImage = null;
		try {
			originalImage = ImageIO.read(new File("Input.jpg"));
		} catch (IOException e1) {
			e1.printStackTrace();
		}
		int w = originalImage.getWidth();
		int h = originalImage.getHeight();
		System.out.println("Image width:\t" + w);
		System.out.println("Image height:\t" + h);
		BufferedImage kmeansImage = new BufferedImage(w, h, originalImage.getType());
		Graphics2D g = kmeansImage.createGraphics();
		g.drawImage(originalImage, 0, 0, w, h, null);
		
		// Read rgb values from the image
		int[] rgb = new int[w * h];
		int count = 0;
		for (int i = 0; i < w; i++) {
			for (int j = 0; j < h; j++) {
				rgb[count++] = kmeansImage.getRGB(i, j);
			}
		}
		
		int[] rgb1 = Arrays.copyOfRange(rgb, 0, rgb.length/2);
		int[] rgb2 = Arrays.copyOfRange(rgb, rgb.length/2, rgb.length);

        point p1 = info.createPoint();
        channel c1 = p1.createChannel();
        p1.execute("KMeans");
        c1.write(rgb1);
        
        point p2 = info.createPoint();
        channel c2 = p2.createChannel();
        p2.execute("KMeans");
        c2.write(rgb2);
        
        System.out.println("Waiting for result...");
        rgb1=(int[]) c1.readObject();
        rgb2=(int[]) c2.readObject();
        
        rgb = Arrays.copyOf(rgb1, rgb1.length + rgb2.length);
        System.arraycopy(rgb2, 0, rgb, rgb1.length, rgb2.length);
	
	    System.out.println("Result found.");
	    
	    count = 0;
	    for (int i = 0; i < w; i++) {
			for (int j = 0; j < h; j++) {
				kmeansImage.setRGB(i, j, rgb[count++]);
			}
		}

	    try{
	  	  ImageIO.write(kmeansImage, "jpg", new File("Res.jpg"));
	    } catch (IOException e) {e.printStackTrace(); return;}
    }
}

import parcs.*;
import java.util.Random;

public class KMeans implements AM
{
	public void run(AMInfo info){
		int k = 10;
		int[] rgb = (int[]) info.parent.readObject();;
		
		int[] k_values = new int[k];
		Random rand = new Random();
		for (int i = 0; i < k_values.length; i++) {
			int random_num;
			boolean contains_duplicate = true;
			if (i == 0) {
				random_num = rand.nextInt(rgb.length);
				k_values[i] = rgb[random_num];
			} else {
				do {
					random_num = rand.nextInt(rgb.length);
					for (int j = 0; j < i; j++) {
						if (j == i - 1 && k_values[j] != rgb[random_num]) {
							// If at the last element in k_values and
							// no duplicates detected
							k_values[i] = rgb[random_num];
							contains_duplicate = false;
						} else if (k_values[j] == rgb[random_num]) {
							// Exit for loop because duplicate color detected
							// and
							// try again with new random number
							j = i;
						}
					}
				} while (contains_duplicate);
			}
			System.out.println("Inital k mean " + i + ": " + k_values[i]);
		}
		
		// Group together similar pixels in the image
		// to corresponding cluster centers.
		// pixel assignments (by their index)
		int[] pixel_assignments = new int[rgb.length];
		int[] num_assignments = new int[k];
		
		// Cluster sums for current cluster values (represented by index)
		int[] alpha_sum = new int[k];
		int[] red_sum = new int[k];
		int[] green_sum = new int[k];
		int[] blue_sum = new int[k];
		
		// iterate until converged. Shouldn't take more than 100
		int max_iterations = 100;
		int num_iterations = 1;
		System.out.println("Clustering k = " + k + " points...");
		while (num_iterations <= max_iterations) {
			if (num_iterations % 10 == 0)
				System.out.println("Iteration:\t" + num_iterations);
			// Clear number of assignments list first
			for (int i = 0; i < k_values.length; i++) {
				num_assignments[i] = 0;
				alpha_sum[i] = 0;
				red_sum[i] = 0;
				green_sum[i] = 0;
				blue_sum[i] = 0;
			}
		
			// Go through all pixels in rgb
			for (int i = 0; i < rgb.length; i++) {
				// Set min_dist initially to infinity (or very large number that
				// wouldn't appear as a distance anyways)
				double min_dist = Double.MAX_VALUE;
				int cluster_index = 0;
				// compare instance's RGB value to each cluster point
				for (int j = 0; j < k_values.length; j++) {
					int a_dist = (getAlpha(rgb[i]) - getAlpha(k_values[j]));
					int r_dist = (getRed(rgb[i]) - getRed(k_values[j]));
					int g_dist = (getGreen(rgb[i]) - getGreen(k_values[j]));
					int b_dist = (getBlue(rgb[i]) - getBlue(k_values[j]));
					double dist = Math.sqrt(a_dist * a_dist + r_dist * r_dist + g_dist * g_dist + b_dist * b_dist);
					if (dist < min_dist) {
						min_dist = dist;
						cluster_index = j;
					}
				}
				// Assign pixel to cluster
				pixel_assignments[i] = cluster_index;
				num_assignments[cluster_index]++;
				// Add pixel's individual argb values to respective sums for use
				// later
				alpha_sum[cluster_index] += getAlpha(rgb[i]);
				red_sum[cluster_index] += getRed(rgb[i]);
				green_sum[cluster_index] += getGreen(rgb[i]);
				blue_sum[cluster_index] += getBlue(rgb[i]);
			}
		
			// update previous assignments list
			for (int i = 0; i < k_values.length; i++) {
				int avg_alpha = (int) ((double) alpha_sum[i] / (double) num_assignments[i]);
				int avg_red = (int) ((double) red_sum[i] / (double) num_assignments[i]);
				int avg_green = (int) ((double) green_sum[i] / (double) num_assignments[i]);
				int avg_blue = (int) ((double) blue_sum[i] / (double) num_assignments[i]);
		
				k_values[i] = ((avg_alpha & 0x000000FF) << 24) | ((avg_red & 0x000000FF) << 16)
						| ((avg_green & 0x000000FF) << 8) | ((avg_blue & 0x000000FF) << 0);
			}
			num_iterations++;
		}
		
		// update RGB array
		for (int i = 0; i < rgb.length; i++) {
			rgb[i] = k_values[pixel_assignments[i]];
		}
		
		System.out.println("Clustering image converged.");
		for (int i = 0; i < k_values.length; i++) {
			System.out.println("Final k mean " + i + ": " + k_values[i]);
		}
	}
	
	// HELPER FUNCTIONS - to get individual R, G, and B values
	public static int getRed(int pix) {return (pix >> 16) & 0xFF;}
	
	public static int getGreen(int pix) {return (pix >> 8) & 0xFF;}
	
	public static int getBlue(int pix) {return pix & 0xFF;}
	
	public static int getAlpha(int pix) {return (pix >> 24) & 0xFF;}
}

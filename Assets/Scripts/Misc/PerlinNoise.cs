using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// http://devmag.org.za/2009/04/25/perlin-noise/
public class PerlinNoise
{
    public float[,] GenerateWhiteNoise(int width, int height)
    {
        float[,] noise = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                noise[i, j] = (float)UnityEngine.Random.value % 1;
            }
        }

        return noise;
    }

    public float[,] GenerateSmoothNoise(float[,] baseNoise, int octave)
    {
        int width = baseNoise.GetLength(0);
        int height = baseNoise.GetLength(1);

        float[,] smoothNoise = new float[width, height];

        int samplePeriod = 1 << octave;
        float sampleFrequency = 1.0f / samplePeriod;

        for (int i = 0; i < width; i++)
        {
            int sample_i0 = (i / samplePeriod) * samplePeriod;
            int sample_i1 = (sample_i0 + samplePeriod) % width;
            float horizontal_blend = (i - sample_i0) * sampleFrequency;

            for (int j = 0; j < height; j++)
            {
                int sample_j0 = (j / samplePeriod) * samplePeriod;
                int sample_j1 = (sample_j0 + samplePeriod) % height;
                float vertical_blend = (j - sample_j0) * sampleFrequency;

                float top = UnityEngine.Mathf.Lerp(baseNoise[sample_i0, sample_j0], baseNoise[sample_i1, sample_j0], horizontal_blend);

                float bottom = UnityEngine.Mathf.Lerp(baseNoise[sample_i0, sample_j1], baseNoise[sample_i1, sample_j1], horizontal_blend);

                smoothNoise[i, j] = UnityEngine.Mathf.Lerp(top, bottom, vertical_blend);
            }
        }

        return smoothNoise;
    }

    public float[,] GeneratePerlinNoise(float[,] baseNoise, int octaveCount)
    {
        int width = baseNoise.GetLength(0);
        int height = baseNoise.GetLength(1);

        float[,,] smoothNoise = new float[octaveCount, width, height];

        float persistance = 0.5f;

        for (int i = 0; i < octaveCount; i++)
        {
            float[,] tempSmoothNoise = GenerateSmoothNoise(baseNoise, i);
            for (int j = 0; j < tempSmoothNoise.GetLength(0); j++)
            {
                for (int k = 0; k < tempSmoothNoise.GetLength(1); k++)
                {
                    smoothNoise[i, j, k] = tempSmoothNoise[j, k];
                }
            }
        }

        float[,] perlinNoise = new float[width, height];
        float amplitude = 1.0f;
        float totalAmplitude = 0.0f;

        for (int octave = octaveCount - 1; octave >= 0; octave--)
        {
            amplitude *= persistance;
            totalAmplitude += amplitude;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    perlinNoise[i, j] += smoothNoise[octave, i, j] * amplitude;
                }
            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                perlinNoise[i, j] /= totalAmplitude;
            }
        }

        return perlinNoise;
    }
}


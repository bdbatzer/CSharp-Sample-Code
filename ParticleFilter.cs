using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CSharpCodeSamples
{
    public abstract class Particle
    {
        public Vector State { get; set; }

        public double Weight { get; set; }

        public abstract void Fitness(in Vector observation);
    }

    public abstract class ParticleFilter
    {
        //  Member: Pi
        //-----------------------------------------------------------------------------------
        /// @brief Mathematical constant Pi
        //-----------------------------------------------------------------------------------
        public const double PI = 3.1415926535897932384626433832795;

        // Static Member: rand
        //-----------------------------------------------------------------------------------------
        /// @brief Random number generator
        //-----------------------------------------------------------------------------------------
        protected static System.Random rand;

        // Member: maxParticles
        //-----------------------------------------------------------------------------------------
        /// @brief Maximum number of particles allowed while running filter
        //-----------------------------------------------------------------------------------------
        protected int maxParticles;

        // Member: particles
        //-----------------------------------------------------------------------------------------
        /// @brief The current particles
        //-----------------------------------------------------------------------------------------
        protected List<Particle> particles;

        // Member: sampleSize
        //-----------------------------------------------------------------------------------------
        /// @brief The number of particles to draw in the next iteration of the algorithm
        //-----------------------------------------------------------------------------------------
        protected int sampleSize;

        // Member: extra
        //-----------------------------------------------------------------------------------------
        /// @brief The number of extra particles to add to the sample in each iteration of the algorithm
        //-----------------------------------------------------------------------------------------
        protected int extra;
        // Member: clock
        //-----------------------------------------------------------------------------------------
        /// @brief The stopwatch object to keep track of elapsed time between updates
        //-----------------------------------------------------------------------------------------
        protected Stopwatch clock;

        // Member: lastUpdateTime
        //-----------------------------------------------------------------------------------------
        /// @brief The last time in which we updated particles
        //-----------------------------------------------------------------------------------------
        protected double lastUpdateTime;

        // Property: Value
        //-----------------------------------------------------------------------------------------
        /// @brief The likelihood/value of the current estimate of state
        //-----------------------------------------------------------------------------------------
        public double Value { get; protected set; }

        // Function: Initialize
        //-----------------------------------------------------------------------------------------
        /// @brief Initializes filter with the maximum number of particles around the current observation
        /// 
        /// @param observation = Current observed bounding box
        //-----------------------------------------------------------------------------------------
        public abstract void Initialize(in Vector observation);

        // Function: Update
        //-----------------------------------------------------------------------------------------
        /// @brief Updates states and weights of current particle set
        /// 
        /// @param observation = Current observed bounding box
        /// @return The current estimate of state
        //-----------------------------------------------------------------------------------------
        public abstract Vector Update(in Vector observation);

        // Function: CreateNewParticles
        //-----------------------------------------------------------------------------------------
        /// @brief Creates new particles for filter
        /// 
        /// @param currBox = Current observed bounding box
        /// @param desiredNum = Desired number of particles 
        //-----------------------------------------------------------------------------------------
        protected abstract void CreateNewParticles(in Vector lowerBounds, in Vector upperBounds, int desiredNum);

        // Function: ResampleParticles
        //-----------------------------------------------------------------------------------------
        /// @brief Low variance resampling method for creating new particle set based on highest 
        /// weighted particles in previous set (from Thrun's Probabilistic Robotics)
        /// 
        /// @param sumWeights = The total of all weights in the current particle set
        //-----------------------------------------------------------------------------------------
        protected void ResampleParticles(double sumWeights)
        {
            rand = new System.Random((int)clock.ElapsedMilliseconds);
            double r = 1.0 / sampleSize * rand.NextDouble();
            double c = particles[0].Weight / sumWeights;
            int i = 0;
            List<Particle> newParticles = new List<Particle>(sampleSize);
            for (int m = 0; m < sampleSize; m++)
            {
                double u = r + ((double)m / sampleSize);
                while (u > c)
                {
                    i++;
                    if (i >= particles.Count) break;
                    c += particles[i].Weight / sumWeights;
                }
                if (i >= particles.Count) break;
                newParticles.Add(particles[i]);
            }

            particles = newParticles;
        }

        // Function: GetSampleSize
        //-----------------------------------------------------------------------------------------
        /// @brief Get number of particles to resample in next step of particle filter
        //-----------------------------------------------------------------------------------------
        public int GetSampleSize()
        {
            return sampleSize;
        }

        // Function: GetNumParticles
        //-----------------------------------------------------------------------------------------
        /// @brief Get total number of particles at the moment
        //-----------------------------------------------------------------------------------------
        public int GetNumParticles()
        {
            return particles.Count;
        }

        // Static Function: RandNormal
        //-----------------------------------------------------------------------------------------
        /// @brief Generates random number from distribution defined by provided mean and standard 
        /// deviation
        /// 
        /// @param mean = The mean of the distribution
        /// @param stdDev = X pixel location that defines the left side of the particle bbox 
        /// @returns Double output of sampling from the distribution
        //-----------------------------------------------------------------------------------------
        protected static double RandNormal(double mean, double stdDev)
        {
            double u1 = 1 - rand.NextDouble();
            double u2 = 1 - rand.NextDouble();
            double randStdNorm = Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * PI * u2);
            return mean + stdDev * randStdNorm;
        }

        // Static Function: RandRange
        //-----------------------------------------------------------------------------------------
        /// @brief Generates random number from the provided range
        /// 
        /// @param min = The lower bound of the distribution
        /// @param max = The upper bound of the distribution
        /// @returns Float output of sampling from the distribution
        //-----------------------------------------------------------------------------------------
        protected static double RandRange(double min, double max)
        {
            return (max - min) * rand.NextDouble() + min;
        }
    }
}

using System;
using System.Diagnostics;

namespace CSharpCodeSamples
{
    // Class: Kalman Filter 
    //-----------------------------------------------------------------------------------------
    /// @brief Kalman Filter for C#
    //-----------------------------------------------------------------------------------------
    public class KalmanFilter
    {
        // Property: p_state
        //-----------------------------------------------------------------------------------------
        /// @brief The state vector 
        //-----------------------------------------------------------------------------------------
        public Vector State { get; private set; }

        // Member: Q
        //-----------------------------------------------------------------------------------------
        /// @brief Process noise covariance matrix
        //-----------------------------------------------------------------------------------------
        public Matrix Q;

        // Member: R
        //-----------------------------------------------------------------------------------------
        /// @brief Measurement noise covariance matrix
        //-----------------------------------------------------------------------------------------
        public Matrix R;

        // Member: P
        //-----------------------------------------------------------------------------------------
        /// @brief Estimate error covariance
        //-----------------------------------------------------------------------------------------
        private Matrix P;

        // Member: F
        //-----------------------------------------------------------------------------------------
        /// @brief System transition matrix
        //-----------------------------------------------------------------------------------------
        private Matrix F;

        // Member: H
        //-----------------------------------------------------------------------------------------
        /// @brief Measurement matrix
        //-----------------------------------------------------------------------------------------
        private Matrix H;

        // Member: I
        //-----------------------------------------------------------------------------------------
        /// @brief Identity matrix
        //-----------------------------------------------------------------------------------------
        private Matrix I;

        // Member: clock
        //-----------------------------------------------------------------------------------------
        /// @brief The stopwatch object to keep track of elapsed time between updates
        //-----------------------------------------------------------------------------------------
        private Stopwatch clock;

        // Member: lastUpdateTime
        //-----------------------------------------------------------------------------------------
        /// @brief The last time in which we updated particles
        //-----------------------------------------------------------------------------------------
        private double lastUpdateTime;

        // Constructor: KalmanFilter
        //-----------------------------------------------------------------------------------------
        /// @brief Constructor for KalmanFilter
        //-----------------------------------------------------------------------------------------
        public KalmanFilter()
        {
            clock = new Stopwatch();
            lastUpdateTime = 0;
        }

        // Function: Initialize
        //-----------------------------------------------------------------------------------------
        /// @brief Initializes the kalman filter
        //-----------------------------------------------------------------------------------------
        public void Initialize(in Vector state, double uncertainty, double systemNoise, double measurementNoise)
        {
            State = new Vector(2 * state.Length);
            Q = new Matrix(2 * state.Length, 2 * state.Length);
            R = new Matrix(state.Length, state.Length);
            H = new Matrix(state.Length, 2 * state.Length);
            P = new Matrix(2 * state.Length, 2 * state.Length);
            for (int i = 0; i < 2 * state.Length; i++)
            {
                if (i < state.Length)
                {
                    State[i] = state[i];
                    R[i, i] = measurementNoise;
                    H[i, i] = 1;
                }
                else State[i] = 0;
                P[i, i] = uncertainty;
                Q[i, i] = systemNoise;
            }
            F = Matrix.Identity(2 * state.Length);
            I = Matrix.Identity(2 * state.Length);

            clock.Restart();
            lastUpdateTime = 0;
        }

        // Function: Update
        //-----------------------------------------------------------------------------------------
        /// @brief Update step of the kalman filter
        //-----------------------------------------------------------------------------------------
        public void Update(in Vector measurement)
        {
            if (measurement.Length != State.Length / 2)
                throw new Exception("Cannot use this measurement with this kalman filter. Mismatch in dimensions!");

            // Predict
            Vector xHat = Predict();

            // Kalman gain
            Vector y = measurement - H * xHat;
            Matrix PHt = P * (H.T);
            Matrix K = PHt * ((H * PHt + R).Inverse());

            // Update
            State = xHat + K * y;
            P = (I - K * H) * P;
        }

        // Function: Predict
        //-----------------------------------------------------------------------------------------
        /// @brief Predict step of the kalman filter
        //-----------------------------------------------------------------------------------------
        public virtual Vector Predict()
        {
            // Get elapsed time
            double currTime = (double)clock.ElapsedMilliseconds / 1000;
            double dt = currTime - lastUpdateTime;
            lastUpdateTime = currTime;

            // Insert time interval into state transition matrix
            for (int i = State.Length / 2; i < State.Length; i++)
            {
                F[i - State.Length / 2, i] = dt;
            }

            // Predict
            P = F * P * (F.T) + Q;
            return F * State;
        }
    }
}

import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import { FiEye, FiEyeOff } from "react-icons/fi";
import workBg from '../assets/work.jpg';

const Login = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [showPassword, setShowPassword] = useState(false);
    const [error, setError] = useState(null);
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        setError(null);

        try {
            const response = await axios.post("https://localhost:7073/api/auth/login", {
                email,
                password,
            });

            const token = response.data.token;
            localStorage.setItem("token", token);

            const decoded = jwtDecode(token);
            const role = decoded.role || decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

            role === "Administrator" ? navigate("/admin") : navigate("/dashboard");
        } catch (err) {
            setError("Invalid email or password. Please try again.");
        }
    };

    return (
        <div className="login-bg d-flex justify-content-center align-items-center min-vh-100"
            style={{
                backgroundImage: `linear-gradient(rgba(0,0,0,0.3), rgba(0,0,0,0.3)), url(${workBg})`,
                backgroundSize: 'cover',
                backgroundRepeat: 'no-repeat',
                backgroundPosition: 'center',
                backgroundAttachment: 'fixed'
            }}
        >
            <div
                className="card shadow-lg border-0 rounded-4 px-4 pt-5 pb-4"
                style={{ width: "100%", maxWidth: "400px", minHeight: "500px", backgroundColor: "#fff" }}
            >
                <div className="text-center mb-4">
                    <h2 className="fw-bold">TeamHub Login</h2>
                    <p className="text-muted small">Welcome back! Please sign in. 💼</p>
                </div>

                {error && (
                    <div className="alert alert-danger rounded-3 py-2 text-center small fade-in">
                        {error}
                    </div>
                )}

                <form onSubmit={handleLogin} className="fade-in">
                    <div className="mb-3">
                        <label className="form-label fw-semibold">Email address</label>
                        <input
                            type="email"
                            className="form-control"
                            placeholder="e.g. user@teamhub.com"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                        />
                    </div>

                    <div className="mb-3">
                        <label className="form-label fw-semibold">Password</label>
                        <div className="position-relative">
                            <input
                                type={showPassword ? "text" : "password"}
                                className="form-control pe-5" 
                                placeholder="Enter your password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                required
                            />
                            <span
                                className="position-absolute"
                                style={{
                                    top: "50%",
                                    right: "12px",
                                    transform: "translateY(-50%)",
                                    cursor: "pointer",
                                    color: "#6c757d",
                                }}
                                onClick={() => setShowPassword((prev) => !prev)}
                            >
                                {showPassword ? <FiEyeOff size={18} /> : <FiEye size={18} />}
                            </span>
                        </div>
                    </div>

                    {/* Remember Me Checkbox */}
                    <div className="form-check mb-4">
                        <input
                            className="form-check-input"
                            type="checkbox"
                            id="rememberMe"
                        />
                        <label className="form-check-label text-muted small" htmlFor="rememberMe">
                            Remember me
                        </label>
                    </div>
                    <button type="submit" className="btn btn-primary w-100 fw-semibold py-2 mt-4">
                        Login
                    </button>
                </form>
            </div>
        </div>
    );
};

export default Login;

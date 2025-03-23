import React from "react";
import { Link, useNavigate } from "react-router-dom";
import { getUserRole } from "../utils/auth";

const Navbar = () => {
    const navigate = useNavigate();
    const token = localStorage.getItem("token");
    const role = getUserRole();

    const handleLogout = () => {
        localStorage.removeItem("token");
        navigate("/login");
    };

    return (
        <nav className="navbar navbar-expand-lg navbar-dark bg-dark shadow-sm">
            <div className="container">
                <Link className="navbar-brand fw-bold" to="/">TeamHub</Link>
                <div className="collapse navbar-collapse">
                    <ul className="navbar-nav ms-auto">
                        {token && (
                            <>
                                {role === "Administrator" && (
                                    <li className="nav-item">
                                        <Link className="nav-link" to="/admin/dashboard">Dashboard</Link>
                                    </li>
                                )}
                                {role === "Employee" && (
                                    <li className="nav-item">
                                        <Link className="nav-link" to="/employee/dashboard">Dashboard</Link>
                                    </li>
                                )}
                                <li className="nav-item">
                                    <button className="btn btn-danger ms-2" onClick={handleLogout}>
                                        Logout
                                    </button>
                                </li>
                            </>
                        )}
                        {!token && (
                            <li className="nav-item">
                                <Link className="nav-link" to="/login"> Login</Link>
                            </li>
                        )}
                    </ul>
                </div>
            </div>
        </nav>
    );
};

export default Navbar;

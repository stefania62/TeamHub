import React from "react";
import { Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar"; 
import Login from "./pages/Login";
import Dashboard from "./pages/Dashboard";
import Tasks from "./pages/Tasks";
import AdminDashboard from "./pages/AdminDashboard";
import ProtectedRoute from "./routes/ProtectedRoute";

const App = () => {
    return (
        <>
            <Navbar /> {}
            <Routes>
                <Route path="/" element={<Login />} />
                <Route path="/login" element={<Login />} />
                <Route path="/dashboard" element={<Dashboard />} />
                <Route path="/tasks" element={<Tasks />} />
                <Route
                    path="/admin"
                    element={<ProtectedRoute element={<AdminDashboard />} allowedRoles={["Admin"]} />}
                />
            </Routes>
        </>
    );
};

export default App;

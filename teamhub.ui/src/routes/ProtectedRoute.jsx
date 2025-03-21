import React from "react";
import { Navigate } from "react-router-dom";
import { getUserRole } from "../utils/auth";

const ProtectedRoute = ({ element, allowedRoles }) => {
    const token = localStorage.getItem("token");
    const role = getUserRole();

    if (!token || (allowedRoles && !allowedRoles.includes(role))) {
        return <Navigate to="/login" replace />;
    }

    return element;
};

export default ProtectedRoute;

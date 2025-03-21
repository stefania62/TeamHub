import React, { useEffect, useState } from "react";
import axios from "axios";
import { API_BASE_URL } from "../api";

const Dashboard = () => {
    const [projects, setProjects] = useState([]);

    useEffect(() => {
        const fetchProjects = async () => {
            try {
                const response = await axios.get(`${API_BASE_URL}/projects`, {
                    headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
                });
                setProjects(response.data);
            } catch (error) {
                console.error("Error fetching projects:", error);
            }
        };

        fetchProjects();
    }, []);

    return (
        <div className="container mt-5">
            <h2 className="mb-4">Your Projects</h2>
            <div className="row">
                {projects.map((project) => (
                    <div key={project.id} className="col-md-4 mb-4">
                        <div className="card shadow-sm">
                            <div className="card-body">
                                <h5 className="card-title">{project.name}</h5>
                                <p className="card-text">Tasks: {project.tasks?.length || 0}</p>
                                <button className="btn btn-primary btn-sm">View Details</button>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default Dashboard;

import React, { useEffect, useState } from "react";
import { getEmployees, getProjects, createProject, deleteProject, assignEmployeeToProject } from "../api";

const AdminDashboard = () => {
    const [projects, setProjects] = useState([]);
    const [employees, setEmployees] = useState([]);
    const [newProject, setNewProject] = useState("");
    const [selectedEmployee, setSelectedEmployee] = useState("");
    const [selectedProject, setSelectedProject] = useState("");
    const [error, setError] = useState("");

    useEffect(() => {
        loadProjects();
        loadEmployees();
    }, []);

    const loadProjects = async () => {
        try {
            const data = await getProjects();
            setProjects(data);
        } catch (error) {
            console.error(error);
        }
    };

    const loadEmployees = async () => {
        try {
            const data = await getEmployees();
            setEmployees(data);
        } catch (error) {
            console.error(error);
        }
    };

    const handleCreateProject = async () => {
        if (!newProject) {
            setError("Project name is required.");
            return;
        }

        try {
            await createProject(newProject);
            setNewProject("");
            loadProjects();
        } catch (error) {
            setError(error.message);
        }
    };

    return (
        <div className="container mt-5">
            <h2>Admin Dashboard</h2>
            {error && <div className="alert alert-danger">{error}</div>}

            {/* Create Project */}
            <div className="card p-3 shadow-sm">
                <h4>Create Project</h4>
                <div className="d-flex">
                    <input
                        type="text"
                        className="form-control"
                        placeholder="Project Name"
                        value={newProject}
                        onChange={(e) => setNewProject(e.target.value)}
                    />
                    <button className="btn btn-primary ms-2" onClick={handleCreateProject}>
                        ➕ Add
                    </button>
                </div>
            </div>

            {/* Projects List */}
            <h4 className="mt-4">📂 All Projects</h4>
            <ul className="list-group">
                {projects.map((project) => (
                    <li key={project.id} className="list-group-item d-flex justify-content-between">
                        <span>{project.name}</span>
                        <button className="btn btn-danger btn-sm">Delete</button>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default AdminDashboard;

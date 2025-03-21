import React, { useEffect, useState } from "react";
import {
    getEmployees,
    getProjects,
    createProject,
    deleteProject,
    createEmployee,
    updateEmployee,
    deleteEmployee,
    assignEmployeeToProject,
    removeEmployeeFromProject,
    createTask,
    deleteTask,
    completeTask,
} from "../api";

const AdminDashboard = () => {
    const [projects, setProjects] = useState([]);
    const [employees, setEmployees] = useState([]);
    const [newEmployeeRole, setNewEmployeeRole] = useState('');
    const [newEmployeeFullname, setNewEmployeeFullname] = useState('');
    const [newEmployeeEmail, setNewEmployeeEmail] = useState('');
    const [newEmployeeUsername, setNewEmployeeUsername] = useState('');
    const [newEmployeePassword, setNewEmployeePassword] = useState('');
    const [editEmployeeRole, setEditEmployeeRole] = useState('');
    const [editEmployeeUsername, setEditEmployeeUsername] = useState('');
    const [editEmployeeEmail, setEditEmployeeEmail] = useState('');
    const [editEmployeeFullname, setEditEmployeeFullname] = useState('');
    const [editEmployeeId, setEditEmployeeId] = useState(null);
    const [newProject, setNewProject] = useState("");
    const [newDescription, setNewDescription] = useState("");  
    const [selectedEmployee, setSelectedEmployee] = useState("");
    const [selectedProject, setSelectedProject] = useState("");
    const [selectedTask, setSelectedTask] = useState("");
    const [newTask, setNewTask] = useState("");
    const [activeTab, setActiveTab] = useState("employees");

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
            setError("Error loading projects.");
        }
    };

    const loadEmployees = async () => {
        try {
            const data = await getEmployees();
            setEmployees(data);
        } catch (error) {
            console.error(error);
            setError("Error loading employees.");
        }
    };

    const handleCreateProject = async () => {
        if (!newProject) {
            setError("Project name is required.");
            return;
        }

        const projectModel = {
            name: newProject,       
            description: newDescription || "", 
        };

        try {
            await createProject(projectModel); 
            setNewProject("");       
            setNewDescription("");   
            loadProjects();          
        } catch (error) {
            setError("Failed to create project.");
        }
    };

    const handleUpdateProject = async () => {
        if (!newProject) {
            setError("Project name is required.");
            return;
        }

        const projectModel = {
            name: newProject,
            description: newDescription || "",
        };

        try {
            await updateProject(selectedProject, projectModel);
            setNewProject("");
            setNewDescription("");
            loadProjects();
        } catch (error) {
            setError("Failed to update project.");
        }
    };
    const handleDeleteProject = async (projectId) => {
        try {
            await deleteProject(projectId);
            loadProjects();
        } catch (error) {
            setError("Failed to delete project.");
        }
    };
    const handleCreateEmployee = async () => {
        console.log("tt");
        if (!newEmployeeEmail || !newEmployeeFullname) {
            setError("Employee name, email, and full name are required.");
            return;
        }

        const employeeModel = {
            username: newEmployeeUsername,        
            email: newEmployeeEmail,         
            fullName: newEmployeeFullname,    
            password: newEmployeePassword,    
            virtualPath: "",                  
            roles: ["Employee"]             
        };
        console.log(employeeModel);
        try {
            await createEmployee(employeeModel);  
            setNewEmployeeEmail('');
            setNewEmployeeFullname('');
            setNewEmployeePassword('');
            setNewEmployeeUsername('');
            setNewEmployeeRole('');
            loadEmployees();  
        } catch (error) {
            setError("Failed to create employee.");
        }
    };

    const handleEditEmployee = (employee) => {
        console.log(employee);
        setEditEmployeeId(employee.id); 
        setEditEmployeeEmail(employee.email);    
        setEditEmployeeFullname(employee.fullname); 
        setEditEmployeeRole(employee.role);  
        setEditEmployeeUsername(employee.username);  
    };

    const handleUpdateEmployee = async () => {
        if (!editEmployeeEmail || !editEmployeeFullname) {
            setError("Employee name, email, and full name are required.");
            return;
        }

        const employeeModel = {
            id: editEmployeeId,
            email: editEmployeeEmail,
            fullName: editEmployeeFullname,
            username: editEmployeeUsername,
            virtualPath: "",
            roles: ["Employee"],
        };

        try {
            await updateEmployee(employeeModel);
            setEditEmployeeId(null);  
            setEditEmployeeFullname("");
            setEditEmployeeUsername("");
            setEditEmployeeEmail("");
            loadEmployees();  
        } catch (error) {
            setError(error);  
        }
    };

    const handleDeleteEmployee = async (employeeId) => {
        try {
            await deleteEmployee(employeeId);
            loadEmployees();
        } catch (error) {
            setError("Failed to delete employee.");
        }
    };

    const handleAssignEmployee = async () => {
        if (!selectedEmployee || !selectedProject) {
            setError("Please select both employee and project.");
            return;
        }
        try {
            await assignEmployeeToProject(selectedProject, selectedEmployee);
            loadProjects();
        } catch (error) {
            setError("Failed to assign employee.");
        }
    };

    const handleRemoveEmployee = async () => {
        if (!selectedEmployee || !selectedProject) {
            setError("Please select both employee and project.");
            return;
        }

        try {
            await removeEmployeeFromProject(selectedProject, selectedEmployee);
            loadProjects();
        } catch (error) {
            setError("Failed to remove employee.");
        }
    };

    const handleCreateTask = async () => {
        if (!newTask || !selectedProject) {
            setError("Task name and project are required.");
            return;
        }

        try {
            await createTask({ name: newTask, projectId: selectedProject });
            setNewTask("");
            loadProjects();
        } catch (error) {
            setError("Failed to create task.");
        }
    };

    const handleDeleteTask = async (taskId) => {
        try {
            await deleteTask(taskId);
            loadProjects();
        } catch (error) {
            setError("Failed to delete task.");
        }
    };

    const handleMarkTaskCompleted = async (taskId) => {
        try {
            await completeTask(taskId);
            loadProjects();
        } catch (error) {
            setError("Failed to mark task as completed.");
        }
    };

    // Tab Content
    const renderTabContent = () => {
        switch (activeTab) {
            case "employees":
                return renderEmployeeManagement();
            case "projects":
                return renderProjectManagement();
            case "tasks":
                return renderTaskManagement();
            default:
                return null;
        }
    };

    // Employee Management
    const renderEmployeeManagement = () => (
        <div className="container mt-5">
            <h2>Employee Management</h2>
            {error && (
                <div className="alert alert-danger">
                    {Array.isArray(error) ? error.join(", ") : error}
                </div>
            )}

            {/* Create Employee */}
            <div className="card p-3 shadow-sm mt-4">
                <h4>Create Employee</h4>
                <div className="d-flex">
                    <input
                        type="text"
                        className="form-control ms-2"
                        placeholder="Employee Email"
                        value={newEmployeeEmail}
                        onChange={(e) => setNewEmployeeEmail(e.target.value)}
                    />
                    <input
                        type="text"
                        className="form-control ms-2"
                        placeholder="Employee Role"
                        value="Role: Employee"  
                        readOnly  
                    />
                    <input
                        type="text"
                        className="form-control ms-2"
                        placeholder="Employee Full Name"
                        value={newEmployeeFullname}
                        onChange={(e) => setNewEmployeeFullname(e.target.value)}
                    />
                    <input
                        type="text"
                        className="form-control ms-2"
                        placeholder="Employee Username"
                        value={newEmployeeUsername}
                        onChange={(e) => setNewEmployeeUsername(e.target.value)}
                    />
                    <input
                        type="password"
                        className="form-control ms-2"
                        placeholder="Employee Password"
                        value={newEmployeePassword}
                        onChange={(e) => setNewEmployeePassword(e.target.value)}
                    />
                    <button className="btn btn-primary ms-2" onClick={handleCreateEmployee}>
                        ➕ Create Employee
                    </button>
                </div>
            </div>

            {/* Edit Employee Form */}
            {editEmployeeId && (
                <div className="card p-3 shadow-sm mt-4">
                    <h4>Edit Employee</h4>
                    <div className="d-flex">
                        <input
                            type="text"
                            className="form-control ms-2"
                            placeholder="Employee Email"
                            value={editEmployeeEmail}
                            onChange={(e) => setEditEmployeeEmail(e.target.value)}
                        />
                        <input
                            type="text"
                            className="form-control ms-2"
                            placeholder="Employee Username"
                            value={editEmployeeUsername}
                            onChange={(e) => setEditEmployeeUsername(e.target.value)}
                        />
                        <input
                            type="text"
                            className="form-control ms-2"
                            placeholder="Employee Role"
                            value="Role: Employee"  
                            readOnly  
                        />
                        <input
                            type="text"
                            className="form-control ms-2"
                            placeholder="Employee Full Name"
                            value={editEmployeeFullname}
                            onChange={(e) => setEditEmployeeFullname(e.target.value)}
                        />
                        <button className="btn btn-success ms-2" onClick={handleUpdateEmployee}>
                            Update Employee
                        </button>
                    </div>
                </div>
            )}

            {/* Employees List */}
            <h4 className="mt-4">Employees List</h4>
            <div className="table-responsive">
                <table className="table table-bordered">
                    <thead>
                        <tr>
                            <th>Role</th>
                            <th>Full Name</th>
                            <th>Username</th>
                            <th>Email</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {employees.map((employee) => (
                            <tr key={employee.id}>
                                <td>{employee.roles}</td>
                                <td>{employee.fullName}</td>
                                <td>{employee.username}</td>
                                <td>{employee.email}</td>
                                <td>
                                    {!employee.roles.includes("Administrator") && (
                                        <>
                                    <button
                                        className="btn btn-warning btn-sm"
                                        onClick={() => handleEditEmployee(employee)}
                                    >
                                        Edit
                                    </button>
                                    <button
                                        className="btn btn-danger btn-sm ms-2"
                                        onClick={() => handleDeleteEmployee(employee.id)}
                                    >
                                        Delete
                                    </button>
                                            </>
                                    )}
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );


    // Project Management
    const renderProjectManagement = () => (
        <div className="container mt-5">
            <h2>Project Management</h2>
            {/* Create Project */}
            <div className="card p-3 shadow-sm mt-4">
                <h4>Create Project</h4>
                <div className="d-flex">
                    <input
                        type="text"
                        className="form-control"
                        placeholder="Project Name"
                        value={newProject}
                        onChange={(e) => setNewProject(e.target.value)}
                    />
                    <input
                        type="text"
                        className="form-control ms-2"
                        placeholder="Project Description"
                        value={newDescription}
                        onChange={(e) => setNewDescription(e.target.value)}
                    />
                    <button className="btn btn-primary ms-2" onClick={handleCreateProject}>
                        ➕ Create Project
                    </button>
                </div>
            </div>

            {/* Projects List */}
            <h4 className="mt-4">Projects List</h4>
            <div className="table-responsive">
                <table className="table table-bordered">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Description</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {projects.map((project) => (
                            <tr key={project.id}>
                                <td>{project.name}</td>
                                <td>{project.description}</td>
                                <td>
                                    <button
                                        className="btn btn-warning btn-sm"
                                        onClick={() => handleUpdateProject(project.id)}
                                    >
                                        Edit
                                    </button>
                                    <button
                                        className="btn btn-danger btn-sm ms-2"
                                        onClick={() => handleDeleteProject(project.id)}
                                    >
                                        Delete
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );

    // Task Management
    const renderTaskManagement = () => (
        <div className="container mt-5">
            <h2>Task Management</h2>
            {/* Create Task */}
            <div className="card p-3 shadow-sm mt-4">
                <h4>Create Task</h4>
                <div className="d-flex">
                    <input
                        type="text"
                        className="form-control"
                        placeholder="Task Name"
                        value={newTask}
                        onChange={(e) => setNewTask(e.target.value)}
                    />
                    <select
                        className="form-select ms-2"
                        value={selectedProject}
                        onChange={(e) => setSelectedProject(e.target.value)}
                    >
                        <option value="">Select Project</option>
                        {projects.map((project) => (
                            <option key={project.id} value={project.id}>
                                {project.name}
                            </option>
                        ))}
                    </select>
                    <button className="btn btn-primary ms-2" onClick={handleCreateTask}>
                        ➕ Create Task
                    </button>
                </div>
            </div>

            {/* Tasks List */}
            <h4 className="mt-4">Tasks List</h4>
            <div className="table-responsive">
                <table className="table table-bordered">
                    <thead>
                        <tr>
                            <th>Task Name</th>
                            <th>Assigned Employee</th>
                            <th>Status</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {projects
                            .filter((project) => project.id === selectedProject)
                            .flatMap((project) =>
                                project.tasks.map((task) => (
                                    <tr key={task.id}>
                                        <td>{task.name}</td>
                                        <td>{task.assignedUserId || "Unassigned"}</td>
                                        <td>{task.status}</td>
                                        <td>
                                            <button
                                                className="btn btn-success btn-sm"
                                                onClick={() => handleMarkTaskCompleted(task.id)}
                                            >
                                                Complete
                                            </button>
                                            <button
                                                className="btn btn-danger btn-sm ms-2"
                                                onClick={() => handleDeleteTask(task.id)}
                                            >
                                                Delete
                                            </button>
                                        </td>
                                    </tr>
                                ))
                            )}
                    </tbody>
                </table>
            </div>
        </div>
    );

    // Tab Buttons
    const handleTabChange = (tab) => {
        setActiveTab(tab);
    };

    return (
        <div className="container mt-5">
            <h2>Admin Dashboard</h2>
            {/* Tabs Navigation */}
            <ul className="nav nav-tabs" role="tablist">
                <li className="nav-item">
                    <a
                        className={`nav-link ${activeTab === "employees" ? "active" : ""}`}
                        onClick={() => handleTabChange("employees")}
                    >
                        Employees
                    </a>
                </li>
                <li className="nav-item">
                    <a
                        className={`nav-link ${activeTab === "projects" ? "active" : ""}`}
                        onClick={() => handleTabChange("projects")}
                    >
                        Projects
                    </a>
                </li>
                <li className="nav-item">
                    <a
                        className={`nav-link ${activeTab === "tasks" ? "active" : ""}`}
                        onClick={() => handleTabChange("tasks")}
                    >
                        Tasks
                    </a>
                </li>
            </ul>

            {/* Tab Content */}
            {renderTabContent()}
        </div>
    );
};

export default AdminDashboard;
import { Avatar, Button, TextField } from "@mui/material";
import { Box } from "@mui/system";
import { useEffect, useState, useRef } from "react";
import { useStore } from "../../api/stores/Store";
import EditIcon from '@mui/icons-material/Edit';
import SaveIcon from '@mui/icons-material/Save';
import { deepOrange } from "@mui/material/colors";
import { useNavigate } from "react-router";
import { toast } from "react-toastify";
import { UserUpdateModel } from "../../models/User";

const EMAIL_REGEX = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/
type Focus = React.FocusEvent<HTMLInputElement | HTMLTextAreaElement, Element>; 

export default function UserProfile() {
    const passwordSecret = "**********";

    const { userStore } = useStore();
    const currentUser = userStore.user;

    const [firstName, setFirstName] = useState(currentUser!.firstName);
    const [firstNameErrors, setFirstNameErrors] = useState('');

    const [lastName, setLastName] = useState(currentUser!.lastName);
    const [lastNameErrors, setLastNameErrors] = useState('');

    const [age, setAge] = useState(currentUser!.age);
    const [ageErrors, setAgeErrors] = useState('');

    const [login, setLogin] = useState(currentUser!.login);
    const [loginErrors, setLoginErrors] = useState('');

    const [password, setPassword] = useState('');
    const [passwordErrors, setPasswordErrors] = useState('');

    const [changeMode, setChangeMode] = useState(true);
    
    useEffect(() => {
        userStore.getCurrentUser();
    }, [currentUser]);
    
    const setProfileChanging = () => {
        setChangeMode(!changeMode);
    }

    const firstNameRef = useRef<HTMLInputElement>();
    const lastNameRef = useRef<HTMLInputElement>();
    const ageRef = useRef<HTMLInputElement>();
    const loginRef = useRef<HTMLInputElement>();
    const passwordRef = useRef<HTMLInputElement>();
    const repeatPasswordRef = useRef<HTMLInputElement>();

    const handleFirstNameChange = (e: Focus) => {
        const firstName = e.target.value;

        if (firstName.length === 0) {
            setFirstNameErrors('First name is required');
        } else {
            setFirstNameErrors('');
        }

        setFirstName(firstName);
    }

    const handleLastNameChange = (e: Focus) => {
        const lastName = e.target.value;

        if (lastName.length === 0) {
            setLastNameErrors('Last name is required');
        } else {
            setLastNameErrors('');
        }

        setLastName(lastName);
    }

    const handleAgeChange = (e: Focus) => {
        const age = Number(e.target.value);

        if(!age) {
            return;
        } else if (age < 10) {
            setAgeErrors('The minimal allowed age is 10');
        } else if (age > 100) {
            setAgeErrors('The maximal allowed age is 100');
        } else {
            setAgeErrors('');
        }

        setAge(String(age));
    }

    const handleEmailChange = (e: Focus) => {
        const email = e.target.value;

        if (email.length === 0) {
            setLoginErrors('Email is required');
        } else if (!email.match(EMAIL_REGEX)) {
            setLoginErrors('Email should be in correct format');
        } else {
            setLoginErrors('');
        }

        setLogin(email);
    }

    const handlePasswordChange = (e: Focus) => {
        const password = e.target.value;

        if (password.length === 0) {
            setPasswordErrors('Please enter password');
        } else {
            setPasswordErrors('');
        }

        setPassword(password);
    }

    const hasErrors = () => {
        // focus inputs
        firstNameRef.current?.focus();
        lastNameRef.current?.focus();
        loginRef.current?.focus();
        passwordRef.current?.focus();
        repeatPasswordRef.current?.focus();
        repeatPasswordRef.current?.blur();

        const isTouched =  firstName.length 
                        && lastName.length 
                        && login.length
                        && password.length;

        const hasAnyError = firstNameErrors.length 
                         || lastNameErrors.length 
                         || loginErrors.length
                         || passwordErrors.length;

        return !isTouched || (isTouched && hasAnyError);
    }

    const submit = async () => {
        if (hasErrors()) {
            return;
        }

        setProfileChanging();

        const user: UserUpdateModel = {
            id: currentUser?.id!,
            login: login, 
            firstName: firstName,
            lastName: lastName, 
            password: password === passwordSecret ? undefined : password
        };

        const result = await userStore.updateUserInfo(user);
        
        if(!result.isSuccessful) {
            toast.error(result.clientErrorMessage);
        } else {
            toast.success('Profile was updated');
        }
    }

    return(
        <Box sx={{
            ml: 3,
            mt: 2
        }}>
            <Box sx={{ 
                "&": {
                    display: "flex",
                    flexDirection: "column",
                    alignItems: "left",
                    width: 260
                },
            }}
            >
                <Avatar sx={{ 
                        width: 240, 
                        height: 240,
                        fontSize: 100,
                        mb: 2,
                        bgcolor: deepOrange[500]
                    }}
                >
                    {currentUser?.firstName[0].toUpperCase()}
                    {currentUser?.lastName[0].toUpperCase()}
                </Avatar>

                <TextField 
                    label="First Name"
                    variant="filled"
                    size="small"
                    helperText={firstNameErrors}
                    value={firstName}
                    required={true}
                    disabled={changeMode}
                    inputRef={firstNameRef}
                    error={firstNameErrors.length !== 0}
                    margin="dense"
                    onFocus={(e) => handleFirstNameChange(e)}
                    onChange={handleFirstNameChange}
                />
                <TextField 
                    label="Last Name"
                    variant="filled"
                    size="small"
                    helperText={lastNameErrors}
                    value={lastName}
                    required={true}
                    disabled={changeMode}
                    inputRef={lastNameRef}
                    error={lastNameErrors.length !== 0}
                    margin="dense"
                    onFocus={(e) => handleLastNameChange(e)}
                    onChange={handleLastNameChange}
                />
                <TextField 
                    label="Age"
                    variant="filled"
                    size="small"
                    helperText={ageErrors}
                    value={age}
                    required={false}
                    disabled={changeMode}
                    inputRef={ageRef}
                    error={ageErrors.length !== 0}
                    margin="dense"
                    onFocus={(e) => handleAgeChange(e)}
                    onChange={handleAgeChange}
                />
                <TextField 
                    label="Email"
                    variant="filled"
                    size="small"
                    helperText={loginErrors}
                    value={login}
                    required={true}
                    disabled={changeMode}
                    inputRef={loginRef}
                    error={loginErrors.length !== 0}
                    margin="dense"
                    onFocus={(e) => handleEmailChange(e)}
                    onChange={handleEmailChange}
                />
                <TextField 
                    label="Password"
                    variant="filled"
                    size="small"
                    helperText={passwordErrors}
                    type="password"
                    value={passwordSecret}
                    required={true}
                    disabled={changeMode}
                    inputRef={passwordRef}
                    error={passwordErrors.length !== 0}
                    margin="dense"
                    onFocus={(e) => handlePasswordChange(e)}
                    onChange={handlePasswordChange}
                />
            </Box>
            <Box sx={{ display: "flex", justifyContent: "space-between", width: 260 }}>
                <Button
                    sx={{ "&:hover": { bgcolor: "orange" }, mt: 1 }}   
                    variant="contained"   
                    onClick={setProfileChanging}
                    disabled={!changeMode}             
                >
                    Change
                    <Avatar sx={{ ml: 1, bgcolor: "white", color: "blue" }}>
                        <EditIcon />
                    </Avatar>
                </Button>

                <Button
                    sx={{ "&:hover": { bgcolor: "orange" }, mt: 1 }}   
                    variant="contained"   
                    onClick={submit} 
                    disabled={changeMode}          
                >
                    Save
                    <Avatar sx={{ ml: 1, bgcolor: "white", color: "blue" }}>
                        <SaveIcon/>
                    </Avatar>
                </Button>
            </Box>
        </Box>
    );
}
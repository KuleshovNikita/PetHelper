import { Box, Button, TextField, Typography } from "@mui/material";
import { useRef, useState } from "react";
import { useNavigate } from "react-router-dom";
import { UserRegisterModel } from "../../models/User";
import { useStore } from "../../api/stores/Store";
import { toast } from 'react-toastify';
import { Result } from "../../models/Result";

const EMAIL_REGEX = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/
type Focus = React.FocusEvent<HTMLInputElement | HTMLTextAreaElement, Element>;

export default function Registration() {
    const [firstName, setFirstName] = useState('');
    const [firstNameErrors, setFirstNameErrors] = useState('');

    const [lastName, setLastName] = useState('');
    const [lastNameErrors, setLastNameErrors] = useState('');

    const [age, setAge] = useState(14);
    const [ageErrors, setAgeErrors] = useState('');

    const [email, setEmail] = useState('');
    const [emailErrors, setEmailErrors] = useState("");

    const [password, setPassword] = useState('');
    const [passwordErrors, setPasswordErrors] = useState("");

    const [repeatPassword, setRepeatPassword] = useState('');
    const [repeatPasswordErrors, setRepeatPasswordErrors] = useState('');

    const firstNameRef = useRef<HTMLInputElement>();
    const lastNameRef = useRef<HTMLInputElement>();
    const ageRef = useRef<HTMLInputElement>();
    const emailRef = useRef<HTMLInputElement>();
    const passwordRef = useRef<HTMLInputElement>();
    const repeatPasswordRef = useRef<HTMLInputElement>();

    const { userStore } = useStore();
    const navigate = useNavigate();

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

        setAge(age);
    }

    const handleEmailChange = (e: Focus) => {
        const email = e.target.value;

        if (email.length === 0) {
            setEmailErrors('Email is required');
        } else if (!email.match(EMAIL_REGEX)) {
            setEmailErrors('Email should be in correct format');
        } else {
            setEmailErrors('');
        }

        setEmail(email);
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

    const handleRepeatPasswordChange = (e: Focus) => {
        const repeatPassword = e.target.value;

        if (repeatPassword.length === 0) {
            setRepeatPasswordErrors('Please enter repeat password');
        } else if (repeatPassword !== password) {
            setRepeatPasswordErrors('Passwords don\'t match');
        } else {
            setRepeatPasswordErrors('');
        }

        setRepeatPassword(repeatPassword);
    }

    const hasErrors = () => {
        // focus inputs
        firstNameRef.current?.focus();
        lastNameRef.current?.focus();
        emailRef.current?.focus();
        passwordRef.current?.focus();
        repeatPasswordRef.current?.focus();
        repeatPasswordRef.current?.blur();

        const isTouched =  firstName.length 
                        && lastName.length 
                        && email.length 
                        && password.length 
                        && repeatPassword.length;

        const hasAnyError = firstNameErrors.length 
                         || lastNameErrors.length 
                         || emailErrors.length 
                         || passwordErrors.length 
                         || repeatPasswordErrors.length;

        return !isTouched || (isTouched && hasAnyError);
    }

    const submit = async () => {
        if (hasErrors()) {
            return;
        }

        const user: UserRegisterModel = {
            login: email, 
            firstName: firstName,
            lastName: lastName, 
            password: password, 
            repeatPassword: repeatPassword
        };

        const result = await userStore.register(user);
        
        if(!result.isSuccessful) {
            toast.error(result.clientErrorMessage);
        } else {
            navigate("/login");
            toast.success('We have sent a verification list to your email, please, follow the instructions there');
        }
    }

    const redirectToLogin = () => {
        navigate("/login", { replace: true });
    }

    return (
        <>
            <Typography variant="h1" 
                        component="h1"
                        align="center">
                Pet Helper
            </Typography>
            <Box
                component="form"
                sx={{
                    '&': {display: 'flex', flexDirection: 'column', alignItems: 'center'},
                    '& .MuiTextField-root': {m: 1, width: '25ch'},
                    '& button': {my: 1},
                    '& .MuiLink-root': {fontSize: '1.2rem'},
                }}
                autoComplete="off"
            >
                <TextField
                    label="First Name"
                    variant="filled"
                    value={firstName}
                    required={true}
                    helperText={firstNameErrors}
                    error={firstNameErrors.length !== 0}
                    inputRef={firstNameRef}
                    onFocus={(e) => handleFirstNameChange(e)}
                    onChange={handleFirstNameChange}
                />
                <TextField
                    label="Last Name"
                    variant="filled"
                    value={lastName}
                    required={true}
                    helperText={lastNameErrors}
                    error={lastNameErrors.length !== 0}
                    inputRef={lastNameRef}
                    onFocus={(e) => handleLastNameChange(e)}
                    onChange={handleLastNameChange}
                />
                <TextField
                    label="Age"
                    variant="filled"
                    value={age}
                    required={false}
                    helperText={ageErrors}
                    error={ageErrors.length !== 0}
                    inputRef={ageRef}
                    onFocus={(e) => handleAgeChange(e)}
                    onChange={handleAgeChange}
                />
                <TextField
                    label="Email"
                    variant="filled"
                    value={email}
                    required={true}
                    helperText={emailErrors}
                    error={emailErrors.length !== 0}
                    inputRef={emailRef}
                    onFocus={(e) => handleEmailChange(e)}
                    onChange={handleEmailChange}
                />
                <TextField
                    label="Password"
                    variant="filled"
                    type="password"
                    value={password}
                    required={true}
                    helperText={passwordErrors}
                    error={passwordErrors.length !== 0}
                    inputRef={passwordRef}
                    onFocus={(e) => handlePasswordChange(e)}
                    onChange={handlePasswordChange}
                />
                <TextField
                    label="Repeat password"
                    variant="filled"
                    type="password"
                    value={repeatPassword}
                    required={true}
                    helperText={repeatPasswordErrors}
                    error={repeatPasswordErrors.length !== 0}
                    inputRef={repeatPasswordRef}
                    onFocus={(e) => handleRepeatPasswordChange(e)}
                    onChange={handleRepeatPasswordChange}
                />
                
                <Button variant="contained" size="large" onClick={submit}>Register</Button>
                <Typography>
                    Or
                </Typography>
                <Button sx={{
                            "&": {
                                backgroundColor: "brown"
                            },
                            "&:hover": {
                                backgroundColor: "red"
                            }
                        }} 
                        variant="contained" 
                        size="large" 
                        onClick={redirectToLogin}>
                    Login
                </Button>
            </Box>
        </>
    )
}

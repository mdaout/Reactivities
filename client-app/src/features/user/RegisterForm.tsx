import React, { useContext } from 'react'
import { Form as FinalForm, Field } from 'react-final-form';
import TextInput from '../../app/common/form/TextInput';
import { Form, Button, Header } from 'semantic-ui-react';
import { RootStoreContext } from '../../app/stores/rootStore';
import { IUserFormValues } from '../../app/models/User';
import { FORM_ERROR } from 'final-form';
import { combineValidators, isRequired } from 'revalidate';
import ErrorMessage from '../../app/common/form/ErrorMessage';

const validate = combineValidators({
    email: isRequired('email'),
    username: isRequired('username'),
    displayName: isRequired('display name'),
    password: isRequired('password')
})

export const RegisterForm = () => {
    const rootStore = useContext(RootStoreContext);
    const { register } = rootStore.userStore;
    return (
        <FinalForm
            onSubmit={(values: IUserFormValues) =>
            register(values).catch(error => ({
            [FORM_ERROR]: error
            }))}

            validate={validate}

            render={({
                handleSubmit,
                submitting,
                form,
                submitError,
                invalid,
                pristine,
                dirtySinceLastSubmit
            }) => (
                    <Form onSubmit={handleSubmit} error>
                        <Header as='h2'
                            content='Signup to Reactivities'
                            color='teal'
                            textAlign='center' />

                        <Field name='username' component={TextInput} placeholder='User Name' />
                        <Field name='displayName' component={TextInput} placeholder='Display Name' />
                        <Field name='email' component={TextInput} placeholder='Email' />
                        <Field name='password'
                            component={TextInput}
                            placeholder='Password'
                            type='password' />
                        {submitError && !dirtySinceLastSubmit && (
                            <ErrorMessage
                                error={submitError}
                               // text={JSON.stringify(submitError.data.errors)}
                                // text='Invalid email or password'
                            />
                        )}

                        <Button disabled={invalid && dirtySinceLastSubmit || pristine}
                            loading={submitting}
                            color='teal'
                            content='Register'
                            fluid
                        />
                        {/* <pre> {JSON.stringify(form.getState(), null, 2)}</pre> */}
                    </Form>
                )}
        />
    )
}
export default RegisterForm;
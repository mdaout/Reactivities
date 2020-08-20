import React, { useContext } from 'react'
import { Modal, Button, Header } from 'semantic-ui-react';
import { RootStore, RootStoreContext } from '../../stores/rootStore';
import { observer } from 'mobx-react-lite';

export const ModalContainer = () => {
    const rootStore = useContext(RootStoreContext)
    const { modal: { open, body }, closeModal } = rootStore.modalStore;
    return (
        <Modal open={open} onClose={closeModal} size='mini'>
            <Modal.Content>{body}</Modal.Content>
        </Modal>
    )
}

export default observer(ModalContainer) ;
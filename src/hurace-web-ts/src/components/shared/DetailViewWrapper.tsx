import React from 'react';
import styled from 'styled-components';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAngleLeft, faPen } from '@fortawesome/free-solid-svg-icons';
import { DefaultLink } from '../../theme/StyledComponents';

const BackLinkPanel = styled.div`
    display: grid;
    grid-template-rows: auto 1fr;
    row-gap: 10px;
    height: 100%;
`;

const TopBar = styled.div`
    display: flex;
`;

const EditText = styled.div`
    margin: auto;
    padding: 0 10px 0 10px;
`;

const EditLink = styled(DefaultLink)`
    margin-left: auto;
    border-radius: 5px;
    background-color: #f0db4f;
    color: black;
    display: flex;
    height: 35px;
`;

const EditIcon = styled(FontAwesomeIcon)`
    margin-right: 5px;
`;

const BackLink = styled(DefaultLink)`
    font-size: 20px;
    display: flex;
`;

const BackIcon = styled(FontAwesomeIcon)`
    margin: auto 5px auto 0;
`;

const BackText = styled.div`
    margin: auto;
    height: fit-content;
`;

export const DetailViewWrapper: React.FC<{
    backText: string;
    url: string;
    editConfig?: {
        editText: string;
        editUrl: string;
    };
}> = ({ backText, url, children, editConfig }) => {
    return (
        <BackLinkPanel>
            <TopBar>
                <BackLink to={url}>
                    <BackIcon icon={faAngleLeft}></BackIcon>
                    <BackText>{backText}</BackText>
                </BackLink>

                {editConfig && (
                    <EditLink to={editConfig.editUrl}>
                        <EditText>
                            <EditIcon icon={faPen} />
                            {editConfig.editText}
                        </EditText>
                    </EditLink>
                )}
            </TopBar>

            {children}
        </BackLinkPanel>
    );
};

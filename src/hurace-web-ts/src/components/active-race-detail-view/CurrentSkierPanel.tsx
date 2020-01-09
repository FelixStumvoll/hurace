import React from 'react';
import { HeaderCard } from '../shared/HeaderCard';
import { SkierListViewItem } from '../skier-list-view/SkierListViewItem';
import styled from 'styled-components';
import { StartList } from '../../models/StartList';

const CurrentSkierWrapper = styled.div`
    margin-left: auto;
`;

const CurrentSkierContent = styled.div`
    display: flex;
`;

const NoSkierText = styled.div`
    margin: auto;
    width: fit-content;
`;

export const CurrentSkierPanel: React.FC<{
    currentSkier: StartList | undefined;
}> = ({ currentSkier }) => {
    return (
        <CurrentSkierWrapper>
            <HeaderCard headerText="Aktueller Läufer">
                <CurrentSkierContent>
                    {currentSkier ? (
                        <SkierListViewItem skier={currentSkier.skier} />
                    ) : (
                        <NoSkierText>Kein Läufer auf der Strecke</NoSkierText>
                    )}
                </CurrentSkierContent>
            </HeaderCard>
        </CurrentSkierWrapper>
    );
};
